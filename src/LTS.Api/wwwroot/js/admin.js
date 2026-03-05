const API = '';

// Auth guard — admin only
(function () {
    const user = JSON.parse(localStorage.getItem('lts_user') || 'null');
    if (!user || user.role !== 0) window.location.href = '/login.html';
})();

function authHeaders() {
    return { 'Authorization': `Bearer ${localStorage.getItem('lts_token')}` };
}

// Tab navigation
document.querySelectorAll('.tab-nav').forEach(link => {
    link.onclick = (e) => {
        e.preventDefault();
        document.querySelectorAll('.admin-section').forEach(s => s.hidden = true);
        const target = link.dataset.section;
        const el = document.getElementById(target);
        if (el) el.hidden = false;
    };
});
// Show first by default
document.querySelectorAll('.admin-section').forEach((s, i) => { if (i > 0) s.hidden = true; });

function fmt(dt) { return new Date(dt).toLocaleDateString('en-US'); }
function fmtT(dt) { return new Date(dt).toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit' }); }
function currency(v) { return '$' + (v ?? 0).toFixed(2); }

async function fetchJson(url) {
    const res = await fetch(url, { headers: authHeaders() });
    if (!res.ok) throw new Error(res.statusText);
    return res.json();
}

// --- Bookings ---
async function loadBookings() {
    const from = document.getElementById('bk-from').value;
    const to = document.getElementById('bk-to').value;
    const type = document.getElementById('bk-type').value;
    const params = new URLSearchParams();
    if (from) params.set('from', from + 'T00:00:00Z');
    if (to) params.set('to', to + 'T23:59:59Z');
    if (type !== '') params.set('type', type === '0' ? 'class' : 'individual');
    const data = await fetchJson(`${API}/api/bookings?${params}`);
    const container = document.getElementById('bookings-table');
    if (!data?.length) { container.innerHTML = '<p>No bookings found.</p>'; return; }
    container.innerHTML = `
        <table>
            <thead><tr><th>Date</th><th>Time</th><th>Type</th><th>Session</th><th>Trainer</th><th>Customer</th><th>Pet</th><th>Price</th></tr></thead>
            <tbody>${data.map(b => `
                <tr>
                    <td>${fmt(b.offering?.startDateTime)}</td>
                    <td>${fmtT(b.offering?.startDateTime)}–${fmtT(b.offering?.endDateTime)}</td>
                    <td>${b.offering?.type === 0 ? 'Class' : 'Individual'}</td>
                    <td>${b.offering?.title ?? ''}</td>
                    <td>${b.offering?.trainer?.displayName ?? '–'}</td>
                    <td>${b.customerName}<br><small>${b.customerEmail}</small></td>
                    <td>${b.pet ? `${b.pet.name} (${b.pet.breed})` : '–'}</td>
                    <td>${currency(b.offering?.price)}</td>
                </tr>`).join('')}
            </tbody>
        </table>`;
}

// --- Revenue ---
async function loadRevenue() {
    const from = document.getElementById('rev-from').value;
    const to = document.getElementById('rev-to').value;
    const params = new URLSearchParams();
    if (from) params.set('from', from + 'T00:00:00Z');
    if (to) params.set('to', to + 'T23:59:59Z');
    const data = await fetchJson(`${API}/api/reports/revenue?${params}`);
    const s = data.summary;
    document.getElementById('revenue-output').innerHTML = `
        <div class="report-summary">
            <strong>Gross Revenue:</strong> ${currency(s.grossRevenue)} &nbsp;|&nbsp;
            <strong>Platform Fee (3%):</strong> ${currency(s.platformFee)} &nbsp;|&nbsp;
            <strong>Trainer Payouts:</strong> ${currency(s.trainerPayouts)} &nbsp;|&nbsp;
            <strong>Session Bookings:</strong> ${s.totalSessionBookings} &nbsp;|&nbsp;
            <strong>Package Purchases:</strong> ${s.totalPackagePurchases}
        </div>
        <table class="report-table">
            <thead><tr><th>Trainer</th><th>Sessions</th><th>Packages</th><th>Session Rev</th><th>Package Rev</th><th>Gross</th><th>Platform Fee</th><th>Payout</th></tr></thead>
            <tbody>${(data.byTrainer ?? []).map(r => `
                <tr>
                    <td>${r.trainerName ?? '–'}</td>
                    <td>${r.sessionBookings}</td>
                    <td>${r.packagePurchases}</td>
                    <td>${currency(r.sessionRevenue)}</td>
                    <td>${currency(r.packageRevenue)}</td>
                    <td>${currency(r.grossRevenue)}</td>
                    <td>${currency(r.platformFee)}</td>
                    <td>${currency(r.trainerPayout)}</td>
                </tr>`).join('')}
            </tbody>
        </table>`;
}

// --- Business ---
async function loadBusiness() {
    const from = document.getElementById('bus-from').value;
    const to = document.getElementById('bus-to').value;
    const params = new URLSearchParams();
    if (from) params.set('from', from + 'T00:00:00Z');
    if (to) params.set('to', to + 'T23:59:59Z');
    const data = await fetchJson(`${API}/api/reports/business?${params}`);
    const out = document.getElementById('business-output');

    const payoutsTable = (data.trainerPayouts ?? []).map(t => `
        <tr><td>${t.trainerName}</td><td>${t.sessionBookings}</td><td>${t.packagePurchases}</td><td>${currency(t.grossRevenue)}</td><td><strong>${currency(t.amountOwed)}</strong></td></tr>
    `).join('');

    const repeats = (data.repeatCustomers ?? []).map(c => `
        <tr><td>${c.name}</td><td>${c.email}</td><td>${c.activityCount}</td><td>${currency(c.totalSpent)}</td></tr>
    `).join('');

    out.innerHTML = `
        <h3>Who I Owe — Total Owed: <strong>${currency(data.totalOwed)}</strong></h3>
        <table><thead><tr><th>Trainer</th><th>Sessions</th><th>Packages</th><th>Gross</th><th>Amount Owed (97%)</th></tr></thead><tbody>${payoutsTable}</tbody></table>
        <h3>Projected Revenue</h3>
        <div class="report-summary">
            Upcoming Bookings: ${data.projectedRevenue?.upcomingBookings} &nbsp;|&nbsp;
            Gross: ${currency(data.projectedRevenue?.grossRevenue)} &nbsp;|&nbsp;
            Platform Fee: ${currency(data.projectedRevenue?.platformFee)}
        </div>
        <h3>Repeat Customers</h3>
        <table><thead><tr><th>Name</th><th>Email</th><th>Activity Count</th><th>Total Spent</th></tr></thead><tbody>${repeats || '<tr><td colspan="4">No repeat customers yet.</td></tr>'}</tbody></table>
    `;
}

// --- Offerings ---
async function loadOfferingsReport() {
    const from = document.getElementById('off-from').value;
    const to = document.getElementById('off-to').value;
    const params = new URLSearchParams();
    if (from) params.set('from', from + 'T00:00:00Z');
    if (to) params.set('to', to + 'T23:59:59Z');
    const data = await fetchJson(`${API}/api/reports/offerings?${params}`);
    const s = data.summary;
    const out = document.getElementById('offerings-rpt-output');
    out.innerHTML = `
        <div class="report-summary">
            Total: ${s.totalOfferings} | At Risk: <span style="color:#e67e22">${s.atRiskCount}</span> |
            Projected Gross: ${currency(s.totalProjectedGross)} | Platform Fee: ${currency(s.totalPlatformFee)}
        </div>
        <table><thead><tr><th>Title</th><th>Trainer</th><th>Type</th><th>Date</th><th>Price</th><th>Booked/Cap</th><th>Min</th><th>Status</th></tr></thead>
        <tbody>${(data.offerings ?? []).map(o => `
            <tr ${o.atRisk ? 'style="background:#fff3cd"' : ''}>
                <td>${o.title}</td><td>${o.trainer ?? '–'}</td>
                <td>${o.type === 0 ? 'Class' : 'Individual'}</td>
                <td>${fmt(o.startDateTime)} ${fmtT(o.startDateTime)}</td>
                <td>${currency(o.price)}</td>
                <td>${o.bookedCount}/${o.capacity}</td>
                <td>${o.minEnrollment}</td>
                <td>${o.atRisk ? '⚠ At Risk' : o.status === 0 ? 'Active' : 'Cancelled'}</td>
            </tr>`).join('')}
        </tbody></table>`;
}

// --- Pets ---
async function loadPets() {
    const breed = document.getElementById('pet-breed-filter').value.trim();
    const params = new URLSearchParams();
    if (breed) params.set('breed', breed);
    const data = await fetchJson(`${API}/api/reports/pets?${params}`);
    const out = document.getElementById('pets-output');
    out.innerHTML = `
        <p>Total: ${data.total}</p>
        <table><thead><tr><th>Name</th><th>Breed</th><th>Neutered</th><th>Spayed</th><th>Owner</th><th>Bookings</th></tr></thead>
        <tbody>${(data.pets ?? []).map(p => `
            <tr><td>${p.name}</td><td>${p.breed}</td>
            <td>${p.isNeutered ? '✔' : ''}</td><td>${p.isSpayed ? '✔' : ''}</td>
            <td>${p.ownerName ?? ''}</td><td>${p.bookingCount}</td></tr>`).join('')}
        </tbody></table>`;
}

// --- Flagged Reviews ---
async function loadFlagged() {
    const data = await fetchJson(`${API}/api/reviews/flagged`);
    const out = document.getElementById('flagged-output');
    if (!data?.length) { out.innerHTML = '<p>No flagged reviews.</p>'; return; }
    out.innerHTML = `
        <table><thead><tr><th>Trainer</th><th>Customer</th><th>Rating</th><th>Comment</th><th>Flag Reason</th><th>Actions</th></tr></thead>
        <tbody>${data.map(r => `
            <tr>
                <td>${r.trainerName ?? '–'}</td><td>${r.customerName}</td>
                <td>${r.rating}★</td><td>${r.comment}</td><td>${r.flagReason ?? ''}</td>
                <td><button onclick="unflag(${r.id})">Unflag</button></td>
            </tr>`).join('')}
        </tbody></table>`;
}

async function unflag(id) {
    await fetch(`${API}/api/reviews/${id}/unflag`, { method: 'POST', headers: authHeaders() });
    loadFlagged();
}

document.getElementById('bk-refresh').onclick = loadBookings;
document.getElementById('rev-generate').onclick = loadRevenue;
document.getElementById('bus-generate').onclick = loadBusiness;
document.getElementById('off-generate').onclick = loadOfferingsReport;
document.getElementById('pets-load').onclick = loadPets;
document.getElementById('flagged-load').onclick = loadFlagged;

loadBookings();
