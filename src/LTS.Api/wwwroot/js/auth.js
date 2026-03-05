const API = '';

document.querySelectorAll('.tab-btn').forEach(btn => {
    btn.onclick = () => {
        document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
        btn.classList.add('active');
        const tab = btn.dataset.tab;
        document.getElementById('login-tab').hidden = tab !== 'login';
        document.getElementById('register-tab').hidden = tab !== 'register';
    };
});

document.getElementById('login-form').onsubmit = async (e) => {
    e.preventDefault();
    const err = document.getElementById('login-error');
    try {
        const res = await fetch(`${API}/api/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email: document.getElementById('login-email').value, password: document.getElementById('login-password').value })
        });
        if (!res.ok) throw new Error(await res.text() || 'Invalid credentials');
        const data = await res.json();
        localStorage.setItem('lts_token', data.token);
        localStorage.setItem('lts_user', JSON.stringify({ id: data.id, email: data.email, role: data.role, firstName: data.firstName, lastName: data.lastName, phone: data.phone }));
        const roles = { 0: '/admin.html', 1: '/trainer.html', 2: '/' };
        window.location.href = roles[data.role] || '/';
    } catch (e) {
        err.textContent = e.message;
        err.hidden = false;
    }
};

document.getElementById('register-form').onsubmit = async (e) => {
    e.preventDefault();
    const err = document.getElementById('register-error');
    try {
        const res = await fetch(`${API}/api/auth/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                email: document.getElementById('reg-email').value,
                password: document.getElementById('reg-password').value,
                role: parseInt(document.getElementById('reg-role').value),
                firstName: document.getElementById('reg-first').value,
                lastName: document.getElementById('reg-last').value,
                phone: document.getElementById('reg-phone').value,
                address: document.getElementById('reg-address').value
            })
        });
        if (!res.ok) throw new Error(await res.text() || 'Registration failed');
        const data = await res.json();
        localStorage.setItem('lts_token', data.token);
        localStorage.setItem('lts_user', JSON.stringify({ id: data.id, email: data.email, role: data.role, firstName: data.firstName, lastName: data.lastName, phone: data.phone }));

        // Auto-create TrainerProfile so the trainer portal works immediately
        if (data.role === 1) {
            await fetch(`${API}/api/trainers`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${data.token}` },
                body: JSON.stringify({
                    userId: data.id,
                    displayName: `${data.firstName} ${data.lastName}`.trim(),
                    bio: '',
                    specialties: '',
                    breedSpecialties: null,
                    photoUrl: null
                })
            });
        }

        const roles = { 0: '/admin.html', 1: '/trainer.html', 2: '/' };
        window.location.href = roles[data.role] || '/';
    } catch (e) {
        err.textContent = e.message;
        err.hidden = false;
    }
};
