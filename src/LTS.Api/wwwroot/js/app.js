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

function formatDate(dt) {
    return new Date(dt).toLocaleDateString('en-US', { weekday: 'short', month: 'short', day: 'numeric', year: 'numeric' });
}

function formatTime(dt) {
    return new Date(dt).toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit' });
}

function stars(rating) {
    if (!rating) return '';
    return '★'.repeat(Math.round(rating)) + '☆'.repeat(5 - Math.round(rating)) + ` (${rating})`;
}

function initNav() {
    const user = getUser();
    const loginLink  = document.getElementById('nav-login');
    const logoutLink = document.getElementById('nav-logout');
    const userSpan   = document.getElementById('header-user');
    const myBookingsLink = document.getElementById('nav-my-bookings');

    if (user) {
        if (loginLink)  loginLink.hidden = true;
        if (userSpan)   { userSpan.hidden = false; userSpan.textContent = `Hi, ${user.firstName}`; }
        if (logoutLink) {
            logoutLink.hidden = false;
            logoutLink.onclick = (e) => { e.preventDefault(); localStorage.clear(); location.reload(); };
        }
        if (myBookingsLink && user.role === 2) myBookingsLink.hidden = false;
    }
}

async function loadOfferings() {
    const typeFilter = document.getElementById('type-filter').value;
    const breedFilter = document.getElementById('breed-filter').value.trim();
    const dateFrom = document.getElementById('date-from').value;
    const dateTo = document.getElementById('date-to').value;

    const params = new URLSearchParams();
    if (typeFilter !== '') params.set('type', typeFilter);
    if (breedFilter) params.set('breed', breedFilter);
    if (dateFrom) params.set('from', dateFrom + 'T00:00:00Z');
    if (dateTo) params.set('to', dateTo + 'T23:59:59Z');

    const data = await fetchJson(`${API}/api/offerings?${params}`);
    const container = document.getElementById('offerings-list');
    container.innerHTML = '';

    if (!data || data.length === 0) {
        container.innerHTML = '<p>No offerings found. Try adjusting filters.</p>';
        return;
    }

    data.forEach(o => {
        const card = document.createElement('div');
        card.className = 'offering-card';
        const typeLabel = o.type === 0 ? 'Class' : 'Individual';
        const canBook = o.spotsLeft > 0 && o.status === 0;
        const atRisk = o.atRisk ? '<span class="at-risk-badge">Low enrollment</span>' : '';
        const breeds = o.allowedBreeds ? `<div class="meta breeds">Breeds: ${escapeHtml(o.allowedBreeds)}</div>` : '';
        const rating = o.averageRating ? `<div class="rating">${stars(o.averageRating)}</div>` : '';

        card.innerHTML = `
            <h3>${escapeHtml(o.title)} ${atRisk}</h3>
            <div class="meta">${typeLabel} · <strong>${escapeHtml(o.trainer?.displayName ?? '')}</strong></div>
            <div class="meta">${formatDate(o.startDateTime)} · ${formatTime(o.startDateTime)}–${formatTime(o.endDateTime)}</div>
            <div class="meta price"><strong>$${o.price}</strong></div>
            ${breeds}
            ${rating}
            <div class="spots">${o.spotsLeft} spot${o.spotsLeft !== 1 ? 's' : ''} left · Min ${o.minEnrollment} to run</div>
            <button class="book-btn" ${!canBook ? 'disabled' : ''}>${canBook ? 'Book Now' : 'Fully Booked'}</button>
        `;

        if (canBook) card.querySelector('.book-btn').onclick = () => showBookingForm(o);
        container.appendChild(card);
    });
}

async function loadPackages() {
    const data = await fetchJson(`${API}/api/packages`);
    const container = document.getElementById('packages-list');
    if (!data || data.length === 0) { container.innerHTML = '<p>No packages currently available.</p>'; return; }

    data.forEach(pkg => {
        const card = document.createElement('div');
        card.className = 'offering-card package-card';
        card.innerHTML = `
            <h3>📦 ${escapeHtml(pkg.title)}</h3>
            <div class="meta">${escapeHtml(pkg.trainer?.displayName ?? '')}</div>
            <div class="meta">${pkg.numSessions} sessions · $${pkg.totalPrice} ($${pkg.pricePerSession}/session)</div>
            <div class="meta">${escapeHtml(pkg.description ?? '')}</div>
            <button class="book-btn" data-id="${pkg.id}">Book Package</button>
        `;
        card.querySelector('.book-btn').onclick = () => showPackageBookingForm(pkg);
        container.appendChild(card);
    });
}

function showBookingForm(offering) {
    const container = document.getElementById('booking-form-container');
    const section = document.getElementById('book');
    section.hidden = false;
    section.scrollIntoView({ behavior: 'smooth' });

    const user = getUser();
    const prefillName = user ? `${user.firstName} ${user.lastName ?? ''}`.trim() : '';
    const prefillEmail = user?.email ?? '';
    const prefillPhone = user?.phone ?? '';

    container.innerHTML = `
        <div class="booking-form">
            <p><strong>${escapeHtml(offering.title)}</strong> — $${offering.price}</p>
            <p class="meta">${formatDate(offering.startDateTime)} · ${formatTime(offering.startDateTime)}–${formatTime(offering.endDateTime)}</p>
            <p class="meta">Trainer: ${escapeHtml(offering.trainer?.displayName ?? '')}</p>
            <label>Your name *</label><input type="text" id="bk-name" value="${escapeHtml(prefillName)}" required>
            <label>Email *</label><input type="email" id="bk-email" value="${escapeHtml(prefillEmail)}" required>
            <label>Phone *</label><input type="tel" id="bk-phone" value="${escapeHtml(prefillPhone)}" required>
            <label>Pet name (optional)</label><input type="text" id="bk-pet">
            <div class="form-actions">
                <button type="button" class="cancel-btn" id="bk-cancel">Cancel</button>
                <button type="button" class="submit-btn" id="bk-submit">Confirm Booking</button>
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
            await postJson(`${API}/api/bookings`, { offeringId: offering.id, customerName: name, customerEmail: email, customerPhone: phone, petId: null });
            container.innerHTML = '<div class="message success">Booking confirmed!</div>';
            loadOfferings();
            if (user) loadMyBookings();
        } catch (e) {
            container.innerHTML += `<div class="message error">${escapeHtml(e.message)}</div>`;
        }
    };
}

function showPackageBookingForm(pkg) {
    const container = document.getElementById('booking-form-container');
    const section = document.getElementById('book');
    section.hidden = false;
    section.scrollIntoView({ behavior: 'smooth' });

    const user = getUser();
    const prefillName = user ? `${user.firstName} ${user.lastName ?? ''}`.trim() : '';
    const prefillEmail = user?.email ?? '';
    const prefillPhone = user?.phone ?? '';

    container.innerHTML = `
        <div class="booking-form">
            <p><strong>${escapeHtml(pkg.title)}</strong> — $${pkg.totalPrice} for ${pkg.numSessions} sessions</p>
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
            container.innerHTML = '<div class="message success">Package purchased! Contact your trainer to schedule your sessions.</div>';
        } catch (e) {
            container.innerHTML += `<div class="message error">${escapeHtml(e.message)}</div>`;
        }
    };
}

async function loadMyBookings() {
    const user = getUser();
    const section = document.getElementById('my-bookings');
    if (!user || !section) return;
    section.hidden = false;

    try {
        const token = getToken();
        const res = await fetch(`${API}/api/bookings/my`, { headers: { 'Authorization': `Bearer ${token}` } });
        if (!res.ok) throw new Error(res.statusText);
        const data = await res.json();
        const container = document.getElementById('my-bookings-list');
        if (!data?.length) { container.innerHTML = '<p>No bookings yet.</p>'; return; }
        container.innerHTML = data.map(b => `
            <div class="offering-card">
                <h3>${escapeHtml(b.offering?.title ?? 'Session')}</h3>
                <div class="meta">${b.offering?.type === 0 ? 'Class' : 'Individual'} · ${escapeHtml(b.offering?.trainer?.displayName ?? '')}</div>
                <div class="meta">${formatDate(b.offering?.startDateTime)} · ${formatTime(b.offering?.startDateTime)}–${formatTime(b.offering?.endDateTime)}</div>
                <div class="meta price"><strong>$${b.offering?.price}</strong></div>
                <div class="meta">Status: <strong>${b.status === 0 ? 'Confirmed' : b.status === 1 ? 'Cancelled' : 'Completed'}</strong></div>
                ${!b.hasReview && b.status !== 1 ? `<button class="book-btn" onclick="showReviewForm(${b.id}, ${b.offering?.trainer?.id ?? 0})">Leave Review</button>` : ''}
            </div>
        `).join('');
    } catch (e) {
        document.getElementById('my-bookings-list').innerHTML = `<p class="error">Could not load bookings.</p>`;
    }
}

function showReviewForm(bookingId, trainerProfileId) {
    const existing = document.getElementById(`review-form-${bookingId}`);
    if (existing) { existing.remove(); return; }
    const btn = document.querySelector(`[onclick="showReviewForm(${bookingId}, ${trainerProfileId})"]`);
    const form = document.createElement('div');
    form.id = `review-form-${bookingId}`;
    form.className = 'booking-form';
    form.style.marginTop = '0.5rem';
    const user = getUser();
    form.innerHTML = `
        <label>Rating</label>
        <select id="rv-rating-${bookingId}">
            <option value="5">★★★★★ Excellent</option>
            <option value="4">★★★★☆ Good</option>
            <option value="3">★★★☆☆ Average</option>
            <option value="2">★★☆☆☆ Poor</option>
            <option value="1">★☆☆☆☆ Terrible</option>
        </select>
        <label>Comment</label>
        <textarea id="rv-comment-${bookingId}" rows="3" placeholder="Share your experience..."></textarea>
        <div class="form-actions" style="margin-top:0.5rem">
            <button class="submit-btn" onclick="submitReview(${bookingId}, ${trainerProfileId}, '${escapeHtml(user?.firstName ?? 'Customer')}')">Submit</button>
        </div>
    `;
    btn.after(form);
}

async function submitReview(bookingId, trainerProfileId, customerName) {
    const rating = parseInt(document.getElementById(`rv-rating-${bookingId}`).value);
    const comment = document.getElementById(`rv-comment-${bookingId}`).value.trim();
    if (!comment) { alert('Please add a comment.'); return; }
    try {
        const token = getToken();
        const res = await fetch(`${API}/api/reviews`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
            body: JSON.stringify({ trainerProfileId, bookingId, customerName, rating, comment })
        });
        if (!res.ok) throw new Error(await res.text());
        document.getElementById(`review-form-${bookingId}`)?.remove();
        document.querySelector(`[onclick="showReviewForm(${bookingId}, ${trainerProfileId})"]`)?.replaceWith(Object.assign(document.createElement('span'), { className: 'meta', textContent: 'Review submitted ✔' }));
    } catch (e) {
        alert(`Failed to submit review: ${e.message}`);
    }
}

function initFromUrlParams() {
    const p = new URLSearchParams(window.location.search);
    if (p.has('type'))  document.getElementById('type-filter').value = p.get('type');
    if (p.has('breed')) document.getElementById('breed-filter').value = p.get('breed');
    if (p.has('from'))  document.getElementById('date-from').value   = p.get('from');
    if (p.has('to'))    document.getElementById('date-to').value     = p.get('to');
}

function init() {
    initNav();
    initFromUrlParams();
    loadOfferings();
    document.getElementById('apply-filters').onclick = loadOfferings;
    loadMyBookings();
}

init();
