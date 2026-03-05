const API = '';
let TRAINER_ID = null;

// Auth guard — trainer only
(function () {
    const user = JSON.parse(localStorage.getItem('lts_user') || 'null');
    if (!user || user.role !== 1) window.location.href = '/login.html';
})();

function authHeaders() {
    return { 'Authorization': `Bearer ${localStorage.getItem('lts_token')}` };
}

document.querySelectorAll('.tab-nav').forEach(link => {
    link.onclick = (e) => {
        e.preventDefault();
        document.querySelectorAll('.trainer-section').forEach(s => s.hidden = true);
        const el = document.getElementById(link.dataset.section);
        if (el) el.hidden = false;
    };
});
document.querySelectorAll('.trainer-section').forEach((s, i) => { if (i > 0) s.hidden = true; });

async function fetchJson(url) {
    const res = await fetch(url, { headers: authHeaders() });
    if (!res.ok) throw new Error(res.statusText);
    return res.json();
}

async function postJson(url, body) {
    const res = await fetch(url, { method: 'POST', headers: { 'Content-Type': 'application/json', ...authHeaders() }, body: JSON.stringify(body) });
    if (!res.ok) throw new Error(await res.text() || res.statusText);
    return res.json();
}

async function putJson(url, body) {
    const res = await fetch(url, { method: 'PUT', headers: { 'Content-Type': 'application/json', ...authHeaders() }, body: JSON.stringify(body) });
    if (!res.ok) throw new Error(await res.text() || res.statusText);
    return res.json();
}

function escapeHtml(text) {
    const div = document.createElement('div'); div.textContent = text ?? ''; return div.innerHTML;
}

function fmt(dt) { return new Date(dt).toLocaleDateString('en-US', { weekday: 'short', month: 'short', day: 'numeric' }); }
function fmtT(dt) { return new Date(dt).toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit' }); }

async function loadProfile(profile) {
    const t = profile ?? await fetchJson(`${API}/api/trainers/${TRAINER_ID}`);
    const container = document.getElementById('profile-form-container');
    container.innerHTML = `
        <div class="profile-form">
            <label>Display Name</label>
            <input type="text" id="pf-name" value="${escapeHtml(t.displayName)}">
            <label>Bio</label>
            <textarea id="pf-bio">${escapeHtml(t.bio)}</textarea>
            <label>Specialties (comma-separated)</label>
            <input type="text" id="pf-spec" value="${escapeHtml(t.specialties)}">
            <label>Breed Specialties (comma-separated)</label>
            <input type="text" id="pf-breeds" value="${escapeHtml(t.breedSpecialties ?? '')}">
            <button type="button" id="pf-save" style="margin-top:1rem">Save Profile</button>
            <div id="pf-msg"></div>
        </div>
    `;
    document.getElementById('pf-save').onclick = saveProfile;
}

async function saveProfile() {
    try {
        await putJson(`${API}/api/trainers/${TRAINER_ID}`, {
            displayName: document.getElementById('pf-name').value,
            bio: document.getElementById('pf-bio').value,
            specialties: document.getElementById('pf-spec').value,
            breedSpecialties: document.getElementById('pf-breeds').value,
            photoUrl: null
        });
        document.getElementById('pf-msg').innerHTML = '<div class="message success">Profile saved.</div>';
    } catch (e) {
        document.getElementById('pf-msg').innerHTML = `<div class="message error">${e.message}</div>`;
    }
}

async function loadOfferings() {
    const data = await fetchJson(`${API}/api/offerings/trainer/${TRAINER_ID}`);
    const container = document.getElementById('my-offerings');
    if (!data?.length) { container.innerHTML = '<p>No sessions yet.</p>'; return; }
    container.innerHTML = data.map(o => `
        <div class="offering-row">
            <div>
                <strong>${escapeHtml(o.title)}</strong> —
                ${fmt(o.startDateTime)} ${fmtT(o.startDateTime)}–${fmtT(o.endDateTime)} ·
                $${o.price} · ${o.bookedCount}/${o.capacity} booked · Min ${o.minEnrollment}
                ${o.atRisk ? '<span class="at-risk-badge">⚠ At Risk</span>' : ''}
            </div>
            <button onclick="editSession(${o.id})">Edit Time</button>
        </div>
    `).join('');
}

async function editSession(id) {
    const data = await fetchJson(`${API}/api/offerings/${id}`);
    const newDate = prompt('New date (YYYY-MM-DD):', new Date(data.startDateTime).toISOString().slice(0, 10));
    if (!newDate) return;
    const newStart = prompt('New start time (HH:MM):', new Date(data.startDateTime).toTimeString().slice(0, 5));
    if (!newStart) return;
    const newEnd = prompt('New end time (HH:MM):', new Date(data.endDateTime).toTimeString().slice(0, 5));
    if (!newEnd) return;
    await putJson(`${API}/api/offerings/${id}`, {
        ...data,
        startDateTime: new Date(`${newDate}T${newStart}:00`).toISOString(),
        endDateTime: new Date(`${newDate}T${newEnd}:00`).toISOString()
    });
    loadOfferings();
}

async function addSlot() {
    const date = document.getElementById('slot-date').value;
    const start = document.getElementById('slot-start').value;
    const end = document.getElementById('slot-end').value;
    const type = parseInt(document.getElementById('slot-type').value, 10);
    const title = document.getElementById('slot-title').value || (type === 0 ? 'Group Class' : 'One-on-One');
    const price = parseFloat(document.getElementById('slot-price').value) || 50;
    const minEnroll = parseInt(document.getElementById('slot-min').value) || 1;
    const cap = parseInt(document.getElementById('slot-cap').value) || 1;
    const breeds = document.getElementById('slot-breeds').value.trim() || null;

    if (!date || !start || !end) { alert('Date, start, and end are required.'); return; }
    if (price < 30) { alert('Minimum price is $30.'); return; }

    await postJson(`${API}/api/offerings`, {
        trainerProfileId: TRAINER_ID, type, title, description: null,
        startDateTime: new Date(`${date}T${start}:00`).toISOString(),
        endDateTime: new Date(`${date}T${end}:00`).toISOString(),
        capacity: cap, minEnrollment: minEnroll, price, location: null, allowedBreeds: breeds
    });
    loadOfferings();
}

async function loadPackages() {
    const data = await fetchJson(`${API}/api/packages/trainer/${TRAINER_ID}`);
    const container = document.getElementById('my-packages');
    if (!data?.length) { container.innerHTML = '<p>No packages yet.</p>'; return; }
    container.innerHTML = data.map(p => `
        <div class="offering-row">
            <strong>${escapeHtml(p.title)}</strong> — ${p.numSessions} sessions · $${p.totalPrice}
            <span>(${p.packageBookings?.length ?? 0} purchased)</span>
        </div>
    `).join('');
}

async function addPackage() {
    const title = document.getElementById('pkg-title').value.trim();
    const desc = document.getElementById('pkg-desc').value.trim() || null;
    const sessions = parseInt(document.getElementById('pkg-sessions').value) || 5;
    const price = parseFloat(document.getElementById('pkg-price').value) || 200;
    if (!title) { alert('Title is required.'); return; }
    await postJson(`${API}/api/packages`, { trainerProfileId: TRAINER_ID, title, description: desc, numSessions: sessions, totalPrice: price });
    loadPackages();
}

async function loadReviews() {
    const data = await fetchJson(`${API}/api/reviews/trainer/${TRAINER_ID}`);
    const out = document.getElementById('reviews-output');
    if (!data?.length) { out.innerHTML = '<p>No reviews yet.</p>'; return; }
    const avg = (data.reduce((s, r) => s + r.rating, 0) / data.length).toFixed(1);
    out.innerHTML = `<p>Average: <strong>${avg}★</strong> from ${data.length} review${data.length !== 1 ? 's' : ''}</p>` +
        data.map(r => `
            <div class="review-item">
                <strong>${'★'.repeat(r.rating)}${'☆'.repeat(5 - r.rating)}</strong> — ${escapeHtml(r.customerName)}<br>
                <em>${escapeHtml(r.comment)}</em>
                <small style="color:#999">${new Date(r.createdAt).toLocaleDateString()}</small>
            </div>
        `).join('');
}

document.getElementById('slot-add').onclick = addSlot;
document.getElementById('pkg-add').onclick = addPackage;

// Bootstrap: resolve trainer profile from JWT, then load everything
(async function init() {
    try {
        const profile = await fetchJson(`${API}/api/trainers/me`);
        TRAINER_ID = profile.id;
        loadProfile(profile);
        loadOfferings();
        loadPackages();
        loadReviews();
    } catch (e) {
        // Profile missing — create it automatically then reload
        if (e.message === 'Not Found') {
            try {
                const user = JSON.parse(localStorage.getItem('lts_user'));
                const res = await fetch(`${API}/api/trainers`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${localStorage.getItem('lts_token')}` },
                    body: JSON.stringify({ userId: user.id, displayName: `${user.firstName} ${user.lastName}`.trim(), bio: '', specialties: '', breedSpecialties: null, photoUrl: null })
                });
                if (res.ok) { window.location.reload(); return; }
            } catch {}
        }
        document.querySelector('main').innerHTML = `<div class="message error" style="margin:2rem">Could not load trainer portal: ${e.message}. Please sign out and sign back in.</div>`;
    }
})();
