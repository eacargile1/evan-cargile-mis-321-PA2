const API = '';
let currentDate = new Date();
let offerings = [];

function initNav() {
    try {
        const user = JSON.parse(localStorage.getItem('lts_user'));
        if (!user) return;
        const loginLink  = document.getElementById('nav-login');
        const userSpan   = document.getElementById('header-user');
        if (loginLink) loginLink.hidden = true;
        if (userSpan)  { userSpan.hidden = false; userSpan.textContent = `Hi, ${user.firstName}`; }
    } catch {}
}
initNav();

async function loadOfferings() {
    const res = await fetch(`${API}/api/offerings`);
    offerings = await res.json();
    renderCalendar();
}

function renderCalendar() {
    const year = currentDate.getFullYear();
    const month = currentDate.getMonth();
    const title = document.getElementById('cal-title');
    title.textContent = new Date(year, month).toLocaleDateString('en-US', { month: 'long', year: 'numeric' });

    const grid = document.getElementById('cal-grid');
    grid.innerHTML = '';

    ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'].forEach(d => {
        const hdr = document.createElement('div');
        hdr.className = 'cal-day-header';
        hdr.textContent = d;
        grid.appendChild(hdr);
    });

    const firstDay = new Date(year, month, 1).getDay();
    const daysInMonth = new Date(year, month + 1, 0).getDate();
    const today = new Date();

    for (let i = 0; i < firstDay; i++) {
        const d = document.createElement('div');
        d.className = 'cal-day other-month';
        grid.appendChild(d);
    }

    for (let day = 1; day <= daysInMonth; day++) {
        const cell = document.createElement('div');
        cell.className = 'cal-day';
        if (today.getFullYear() === year && today.getMonth() === month && today.getDate() === day)
            cell.classList.add('today');

        const dayNum = document.createElement('div');
        dayNum.className = 'day-num';
        dayNum.textContent = day;
        cell.appendChild(dayNum);

        const cellDate = new Date(year, month, day);
        const dayOfferings = offerings.filter(o => {
            const d = new Date(o.startDateTime);
            return d.getFullYear() === year && d.getMonth() === month && d.getDate() === day;
        });

        dayOfferings.forEach(o => {
            const ev = document.createElement('div');
            const isFull = o.spotsLeft <= 0;
            ev.className = `cal-event ${o.type === 0 ? 'class' : ''} ${isFull ? 'full' : ''}`;
            ev.textContent = `${new Date(o.startDateTime).toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit' })} ${o.title}`;
            cell.appendChild(ev);
        });

        cell.onclick = () => showDayDetail(cellDate, dayOfferings);
        grid.appendChild(cell);
    }
}

function showDayDetail(date, dayOfferings) {
    const detail = document.getElementById('cal-day-detail');
    detail.hidden = false;
    document.getElementById('detail-date').textContent = date.toLocaleDateString('en-US', { weekday: 'long', month: 'long', day: 'numeric', year: 'numeric' });

    const container = document.getElementById('detail-sessions');
    if (dayOfferings.length === 0) {
        container.innerHTML = '<p>No sessions on this day.</p>';
        return;
    }
    container.innerHTML = dayOfferings.map(o => {
        const start = new Date(o.startDateTime).toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit' });
        const end = new Date(o.endDateTime).toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit' });
        const typeLabel = o.type === 0 ? 'Class' : 'Individual';
        const spots = o.spotsLeft > 0 ? `${o.spotsLeft} spot${o.spotsLeft !== 1 ? 's' : ''} left` : 'Full';
        return `
            <div class="detail-session">
                <div>
                    <strong>${o.title}</strong><br>
                    <small>${typeLabel} · ${start}–${end} · ${o.trainer?.displayName ?? ''} · $${o.price}</small>
                </div>
                <span>${spots}</span>
            </div>
        `;
    }).join('');
    detail.scrollIntoView({ behavior: 'smooth' });
}

document.getElementById('prev-month').onclick = () => { currentDate.setMonth(currentDate.getMonth() - 1); renderCalendar(); };
document.getElementById('next-month').onclick = () => { currentDate.setMonth(currentDate.getMonth() + 1); renderCalendar(); };

loadOfferings();
