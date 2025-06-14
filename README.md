# ESP32-Controlled_Smart_Device_Interface_via_C-Sharp_and_MySQL
# Türkçe
Bu proje, ESP32 ile sensör verilerinin okunması ve C# Windows Form uygulaması üzerinden kontrol edilmesini sağlayan bir IoT tabanlı LED/SERVO kontrol sistemidir. Veriler MySQL veritabanı aracılığıyla C# uygulaması ile ESP32 arasında senkronize edilir.
🧩 Proje Bileşenleri:
    🔌 Arduino/ESP32 (PROJECT_FINAL.ino):
    LED durumu, sıcaklık, nem ve servo açı verilerini okuyarak MySQL veritabanına gönderir/alır.
    🖥️ C# Windows Forms Uygulaması (Form1.cs):
        Giriş ekranı (admin: admin / 1234)
        LED kontrolü (aç/kapat)
        Servo kontrolü (0-180 derece kaydırıcı)
        Sıcaklık ve nem değerlerini veritabanından okuma
        Gerçek zamanlı saat ve tarih gösterimi
    🗄️ MySQL Veritabanı:
        Veritabanı: esp32_mc_db
        Tablo: esp32_table_test (id, LED_01, Temperature, Servo, Humidity)
🛠️ Kullanım:
    Arduino/ESP32 cihazında PROJECT_FINAL.ino yüklenir.
    Veritabanı oluşturulur ve yapılandırılır.
    Form1.cs içeren C# uygulaması Visual Studio üzerinden çalıştırılır.
    
# Enlish
This project is an IoT-based control system that connects an ESP32 microcontroller with a C# Windows Forms GUI via a MySQL database. The system allows remote monitoring and control of a servo motor and an LED, as well as reading temperature and humidity values.
🧩 Project Components:
    🔌 Arduino/ESP32 (PROJECT_FINAL.ino):
    Sends/receives LED status, temperature, humidity, and servo angle to/from the database.
    🖥️ C# Windows Forms Application (Form1.cs):
        Login screen (admin: admin / 1234)
        LED ON/OFF control
        Servo angle adjustment (scroll bar)
        Live reading of temperature and humidity
        Real-time clock display
    🗄️ MySQL Database:
        Database: esp32_mc_db
        Table: esp32_table_test (id, LED_01, Temperature, Servo, Humidity)
🛠️ Usage:
    Upload PROJECT_FINAL.ino to the ESP32 board.
    Set up the MySQL database and table.
    Run the Visual Studio solution (MySQL_C_LED.sln) to control and monitor the system.
