using System;

namespace BorcTakipApi.Models
{
    /// <summary>
    /// Borç veya alacak işlemlerini takip eden varlık sınıfı.
    /// </summary>
    public class Borc
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Borcun alındığı veya verildiği kişinin ismi.
        /// </summary>
        public string Isim { get; set; } = string.Empty;
        
        /// <summary>
        /// İşlemin parasal değeri (TL).
        /// </summary>
        public double Miktar { get; set; }
        
        /// <summary>
        /// İşlemle ilgili ek detaylar (Opsiyonel).
        /// </summary>
        public string Aciklama { get; set; } = string.Empty;
        
        /// <summary>
        /// İşlemin yönü: 'Alinan' veya 'Verilen'
        /// </summary>
        public string Tip { get; set; } = "Alinan"; 
        
        /// <summary>
        /// İşlemi yapan kullanıcının Id'si (Yabancı anahtar).
        /// </summary>
        public int UserId { get; set; }             
        
        /// <summary>
        /// İşlemin kaydedildiği tarih ve saat.
        /// </summary>
        public DateTime Tarih { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Bu borcun/alacağın kapanıp kapanmadığını belirtir.
        /// </summary>
        public bool IsPaid { get; set; } = false; 
    }
}
