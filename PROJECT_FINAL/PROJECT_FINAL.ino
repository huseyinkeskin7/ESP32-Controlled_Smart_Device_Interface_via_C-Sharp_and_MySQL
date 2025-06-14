#include <WiFi.h>
#include <HTTPClient.h>
#include <ESP32Servo.h> // ESP32Servo kütüphanesini ekleyin
#include <DHT.h> // DHT11 sensörü için kütüphane ekleyin

#define DHTPIN 14     // DHT11 sensörünün bağlı olduğu pin (35 olarak güncellendi)
#define DHTTYPE DHT11 // DHT11 sensörünün tipi

#define LED_D8 13
#define SERVO_PIN 33 // Servo motorunun bağlı olduğu pin

String URL = "http://192.168.1.127/ESP32_MySQL_Database/DHT.php";
const char* host = "http://192.168.1.127/"; // Web sunucu adresi

const char* ssid = "Zyxel_1691";
const char* password = "XMTPPKXUKN";

String prevvalue = "";
String prevvalueservo = "";
unsigned long lastTempSendTime = 0; // Son sıcaklık gönderme zamanı
const unsigned long tempSendInterval = 2000; // Sıcaklık gönderme aralığı (milisaniye cinsinden)

HTTPClient http;
Servo myServo; // ESP32Servo nesnesini oluşturun
DHT dht(DHTPIN, DHTTYPE); // DHT nesnesini oluşturun

void connectWiFi() {
  WiFi.mode(WIFI_OFF);
  delay(1000);
  WiFi.mode(WIFI_STA);
  
  WiFi.begin(ssid, password);
  Serial.println("Connecting to WiFi");
  
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
    
  Serial.println("connected to :  " + String(ssid));
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());
}

void setup() {
  Serial.begin(115200);
  connectWiFi();
  pinMode(LED_D8, OUTPUT);
  digitalWrite(LED_D8, LOW);
  myServo.attach(SERVO_PIN); // Servo nesnesini SERVO_PIN'e bağlayın
  dht.begin(); // DHT sensörünü başlatın
}

void loop() {
  if (WiFi.status() != WL_CONNECTED) {
    connectWiFi();
  }
  unsigned long currentTime = millis();

  if (currentTime - lastTempSendTime >= tempSendInterval) {
    send_temp();
    lastTempSendTime = currentTime;
  }

  if (led_read()) {
    led_read();
  }

  servocont(); // servocont fonksiyonunu çağır
}

bool led_read() {
  HTTPClient http;
  String getAddress = "ESP32_MySQL_Database/GetData.php";
  String linkGet = host + getAddress; 

  http.begin(linkGet);
  http.addHeader("Content-Type", "application/x-www-form-urlencoded");

  int httpCodeGet = http.POST("id=1");
  String payloadGet = http.getString();

  http.end();

  if (prevvalue != payloadGet) {
    if (payloadGet == "1") {
      Serial.print("Sunucudan Gelen Veri: ");
      Serial.println(payloadGet);
      digitalWrite(LED_D8, HIGH); 
      prevvalue = payloadGet;
    } else if (payloadGet == "0") {
      Serial.print("Sunucudan Gelen Veri: ");
      Serial.println(payloadGet);
      digitalWrite(LED_D8, LOW); 
      prevvalue = payloadGet;
    }
    return true; // LED durumu değiştiğinde true döndür
  }
  return false; // LED durumu değişmediğinde false döndür
}

void servocont() {
  HTTPClient http;
  String getAddress = "ESP32_MySQL_Database/GetDataServo.php";
  String linkGet = host + getAddress;

  http.begin(linkGet);
  http.addHeader("Content-Type", "application/x-www-form-urlencoded");

  int httpCodeGet = http.POST("id=1");
  String payloadGetServo = http.getString();

  http.end();

  if (prevvalueservo != payloadGetServo) {
    // Yeni veri eski veriyle aynı değil, işlem yapılabilir
    // Örneğin, yeni veriyi servopozisyonu olarak al ve servoya gönder
    int servoPosition = payloadGetServo.toInt();
    if (servoPosition >= 0 && servoPosition <= 180) {
      // Servo pozisyonu geçerli ise servoya bu pozisyonu gönder
      myServo.write(servoPosition); // Servo motorunu belirtilen açıya döndür
      Serial.print("Servo position from the server: ");
      Serial.println(servoPosition);
    } else {
      Serial.println("Invalid Servo Position!");
    }
    // Yeni veriyi eski veri olarak sakla
    prevvalueservo = payloadGetServo;
  } else {
    // Yeni veri eski veriyle aynı, işlem yapma ve fonksiyondan çık
    return;
  }
}

void send_temp() {
  float temperature = dht.readTemperature(); // Sıcaklık değerini DHT sensöründen al
  float humidity = dht.readHumidity(); // Nem değerini DHT sens,öründen al

  String postData = "Temp=" + String(temperature) + "&Humidity=" + String(humidity);
  
  http.begin(URL);
  http.addHeader("Content-Type", "application/x-www-form-urlencoded");
  
  int httpCode = http.POST(postData);
  String payload = http.getString();

  Serial.print("URL : ");
  Serial.println(URL); 
  Serial.print("Data: ");
  Serial.println(postData);
  Serial.print("HTTP Kodu: ");
  Serial.println(httpCode);
  Serial.print("Payload: ");
  Serial.println(payload);
  Serial.println("--------------------------------------------------");

  http.end();
}
