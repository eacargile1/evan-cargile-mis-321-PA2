const API = '';

function getUser() {
    try { return JSON.parse(localStorage.getItem('lts_user')); } catch { return null; }
}
function getToken() { return localStorage.getItem('lts_token'); }

async function fetchJson(url) {
    const res = await fetch(url);
    if (!res.ok) throw new Error(res.statusText);
    return res.json();
}

async function postJson(url, body) {
    const res = await fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${getToken()}` },
        body: JSON.stringify(body)
    });
    if (!res.ok) throw new Error(await res.text() || res.statusText);
    return res.json();
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text ?? '';
    return div.innerHTML;
}

function initNav() {
    const user = getUser();
    const loginLink  = document.getElementById('nav-login');
    const logoutLink = document.getElementById('nav-logout');
    const userSpan   = document.getElementById('header-user');
    const myBookingsLink = document.getElementById('nav-my-bookings-link');

    if (user) {
        if (loginLink)  loginLink.hidden = true;
        if (userSpan)   { userSpan.hidden = false; userSpan.textContent = `Hi, ${user.firstName}`; }
        if (logoutLink) {
            logoutLink.hidden = false;
            logoutLink.onclick = (e) => { e.preventDefault(); localStorage.clear(); location.reload(); };
        }
        if (myBookingsLink) myBookingsLink.hidden = false;
    }
}

async function loadPackages() {
    const data = await fetchJson(`${API}/api/packages`);
    const container = document.getElementById('packages-list');
    if (!data || data.length === 0) {
        container.innerHTML = '<p class="empty-state">No packages currently available — check back soon!</p>';
        return;
    }

    data.forEach(pkg => {
        const card = document.createElement('div');
        card.className = 'offering-card';
        card.innerHTML = `
            <div class="pkg-sessions-badge">${pkg.numSessions}-Session Package</div>
            <h3>${escapeHtml(pkg.title)}</h3>
            <div class="meta" style="margin-bottom:0.25rem">Trainer: <strong>${escapeHtml(pkg.trainer?.displayName ?? '')}</strong></div>
            <div class="meta" style="margin-bottom:0.5rem">${escapeHtml(pkg.description ?? '')}</div>
            <div class="pkg-pricing">
                <span class="pkg-total">$${pkg.totalPrice}</span>
                <span class="pkg-per">$${pkg.pricePerSession}/session</span>
            </div>
            <button class="book-btn">Purchase Package</button>
        `;
        card.querySelector('.book-btn').onclick = () => showPackageBookingForm(pkg);
        container.appendChild(card);
    });
}

function showPackageBookingForm(pkg) {
    const container = document.getElementById('booking-form-container');
    const section = document.getElementById('book');
    section.hidden = false;
    section.closest('.packages-form-wrap')?.scrollIntoView({ behavior: 'smooth' });

    const user = getUser();
    const prefillName = user ? `${user.firstName} ${user.lastName ?? ''}`.trim() : '';
    const prefillEmail = user?.email ?? '';
    const prefillPhone = user?.phone ?? '';

    container.innerHTML = `
        <div class="booking-form">
            <p><strong>${escapeHtml(pkg.title)}</strong></p>
            <p class="meta">${pkg.numSessions} sessions · $${pkg.totalPrice} total ($${pkg.pricePerSession}/session)</p>
            <p class="meta">Trainer: ${escapeHtml(pkg.trainer?.displayName ?? '')}</p>
            <label>Your name *</label><input type="text" id="bk-name" value="${escapeHtml(prefillName)}" required>
            <label>Email *</label><input type="email" id="bk-email" value="${escapeHtml(prefillEmail)}" required>
            <label>Phone *</label><input type="tel" id="bk-phone" value="${escapeHtml(prefillPhone)}" required>
            <div class="form-actions">
                <button type="button" class="cancel-btn" id="bk-cancel">Cancel</button>
                <button type="button" class="submit-btn" id="bk-submit">Purchase Package</button>
            </div>
        </div>
    `;

    document.getElementById('bk-cancel').onclick = () => { section.hidden = true; container.innerHTML = ''; };
    document.getElementById('bk-submit').onclick = async () => {
        const name = document.getElementById('bk-name').value.trim();
        const email = document.getElementById('bk-email').value.trim();
        const phone = document.getElementById('bk-phone').value.trim();
        if (!name || !email || !phone) { alert('Name, email, and phone are required.'); return; }
        try {
            await postJson(`${API}/api/packages/${pkg.id}/book`, { customerName: name, customerEmail: email, customerPhone: phone, petId: null });
            container.innerHTML = '<div class="message success">Package purchased! Your trainer will reach out to schedule your sessions.</div>';
        } catch (e) {
            container.innerHTML += `<div class="message error">${escapeHtml(e.message)}</div>`;
        }
    };
}

function init() {
    initNav();
    loadPackages();
}

init();
