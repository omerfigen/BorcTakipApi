const API = "/api";
let currentUser = JSON.parse(localStorage.getItem("user"));

// Kullanıcı giriş işlemi
async function login() {
    const u = document.getElementById("user").value.trim();
    const p = document.getElementById("pass").value.trim();
    
    if (!u || !p) { 
        alert("Lütfen kullanıcı adı ve şifre giriniz!"); 
        return; 
    }
    
    try {
        const res = await fetch(`${API}/auth/login`, { 
            method: 'POST', 
            headers: { 'Content-Type': 'application/json' }, 
            body: JSON.stringify({ username: u, password: p }) 
        });
        
        if (res.ok) { 
            const data = await res.json();
            localStorage.setItem("user", JSON.stringify(data)); 
            location.reload(); 
        } else {
            const err = await res.json();
            alert(err.message || "Giriş başarısız, bilgileri kontrol ediniz.");
        }
    } catch (error) {
        console.error("Giriş hatası:", error);
        alert("Sunucuya bağlanırken bir hata oluştu.");
    }
}

// Yeni kullanıcı kayıt işlemi
async function register() {
    const u = document.getElementById("user").value.trim();
    const p = document.getElementById("pass").value.trim();
    
    if (!u || !p) { 
        alert("Kayıt olmak için kullanıcı adı ve şifre zorunludur!"); 
        return; 
    }
    if (u.length < 3) { 
        alert("Kullanıcı adı en az 3 karakter olmalıdır."); 
        return; 
    }
    if (p.length < 3) { 
        alert("Şifre en az 3 karakter olmalıdır."); 
        return; 
    }
    
    try {
        const res = await fetch(`${API}/auth/register`, { 
            method: 'POST', 
            headers: { 'Content-Type': 'application/json' }, 
            body: JSON.stringify({ username: u, password: p }) 
        });
        
        if (res.ok) {
            alert("Kaydınız başarıyla oluşturuldu! Şimdi giriş yapabilirsiniz.");
            document.getElementById("user").value = "";
            document.getElementById("pass").value = "";
        } else {
            const err = await res.json();
            alert(err.message || "Kayıt işlemi başarısız oldu.");
        }
    } catch (error) {
        console.error("Kayıt hatası:", error);
        alert("Sunucuyla iletişim kurulamadı.");
    }
}

// Sistemden çıkış
function logout() { 
    localStorage.removeItem("user"); 
    location.reload(); 
}

// Yeni borç/alacak kaydetme
async function save() {
    const isim = document.getElementById("f-isim").value.trim();
    const miktar = document.getElementById("f-miktar").value.trim();
    const aciklama = document.getElementById("f-aciklama").value.trim();
    const tip = document.getElementById("f-tip").value;

    if (!isim || !miktar || parseFloat(miktar) <= 0) {
        alert("Lütfen geçerli bir isim ve miktar giriniz.");
        return;
    }

    try {
        const res = await fetch(`${API}/borc`, { 
            method: 'POST', 
            headers: { 'Content-Type': 'application/json' }, 
            body: JSON.stringify({ 
                isim: isim, 
                miktar: parseFloat(miktar), 
                aciklama: aciklama || "", 
                tip: tip, 
                userId: currentUser.id,
                isPaid: false
            }) 
        });

        if (res.ok) {
            // Formu temizle
            document.getElementById("f-isim").value = ""; 
            document.getElementById("f-miktar").value = ""; 
            document.getElementById("f-aciklama").value = "";
            // Listeyi güncelle
            init(); 
        } else {
            alert("İşlem kaydedilirken bir sorun oluştu.");
        }
    } catch (error) {
        console.error("Kaydetme hatası:", error);
    }
}

// İşlem durumunu (Ödendi/Ödenmedi) değiştirme
async function toggleStatus(id) {
    try {
        await fetch(`${API}/borc/toggle/${id}`, { method: 'POST' });
        init();
    } catch (error) {
        console.error("Durum güncelleme hatası:", error);
    }
}

// Uygulamayı başlatan ve verileri yükleyen ana metod
async function init() {
    if (!currentUser) return;
    
    // UI durumunu güncelle
    document.getElementById("loginArea").style.display = "none";
    document.getElementById("appArea").style.display = "block";
    document.getElementById("hello").innerText = "Sayın " + currentUser.username;

    try {
        // Borç/Alacak listesini çek
        const res = await fetch(`${API}/borc/${currentUser.id}`);
        const data = await res.json();
        
        const listAlinan = document.getElementById("l-Alinan");
        const listVerilen = document.getElementById("l-Verilen");
        
        listAlinan.innerHTML = ""; 
        listVerilen.innerHTML = "";

        data.forEach(x => {
            const isAlinan = x.tip === "Alinan";
            const btnText = isAlinan ? (x.isPaid ? "ÖDENDİ ✓" : "ÖDENMEDİ") : (x.isPaid ? "ALINDI ✓" : "ALINMADI");
            const btnClass = x.isPaid ? "paid" : "unpaid";
            const html = `
                <div class="item ${x.tip} ${x.isPaid ? 'is-paid' : ''}">
                    <button class="btn-status ${btnClass}" onclick="toggleStatus(${x.id})" title="Durumu değiştirmek için tıklayın">${btnText}</button>
                    <span><b>${x.isim}</b>: ${x.miktar.toLocaleString('tr-TR')} TL</span><br>
                    <span class="desc-text">${x.aciklama || 'Açıklama yok'}</span>
                    <small style="color:#999; display:block; margin-top:5px;">${new Date(x.tarih).toLocaleDateString('tr-TR')}</small>
                </div>`;
                
            if(isAlinan) {
                listAlinan.innerHTML += html;
            } else {
                listVerilen.innerHTML += html;
            }
        });

        // İstatistikleri çek ve güncelle
        const resS = await fetch(`${API}/stats/${currentUser.id}`);
        const s = await resS.json();
        
        document.getElementById("s-borc").innerText = s.totalAlinan.toLocaleString('tr-TR') + " TL";
        document.getElementById("s-alacak").innerText = s.totalVerilen.toLocaleString('tr-TR') + " TL";
        
        const netDurum = document.getElementById("s-net");
        netDurum.innerText = s.netStatus.toLocaleString('tr-TR') + " TL";
        netDurum.style.color = s.netStatus >= 0 ? "var(--success)" : "var(--danger)";
        
    } catch (error) {
        console.error("Veri yükleme hatası:", error);
    }
}

// Uygulama yüklendiğinde çalıştır
document.addEventListener('DOMContentLoaded', () => {
    init();
});
