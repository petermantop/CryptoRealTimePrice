# **Crypto Realtime Price Service - Documentation**

## **Overview**

This document provides details about the **Crypto Realtime Price Service**, which is a .NET 8-based application providing **REST API and WebSocket endpoints** for retrieving **live financial instrument prices** sourced from **Tiingo API**.

The service efficiently manages **over 1,000 WebSocket subscribers** and logs necessary events and errors.

---

## **Installation & Setup**

### **🔹 Run Using Docker**

```sh
docker-compose up --build
```

### **🔹 Access API Documentation (Swagger)**

```sh
http://localhost:5235/swagger/index.html
```

### **🔹 API Base URL**

```
http://localhost:5235/
```

---

## **1️⃣ REST API Endpoints**

### **📌 Get a List of Available Crypto Pairs**

- **Endpoint:** `GET /api/CryptoPairs`
- **Description:** Retrieves the list of supported cryptocurrency pairs.
- **Response:**

```json
[
  { "ticker": "btcusd", "description": "Bitcoin to USD" },
  { "ticker": "ethusd", "description": "Ethereum to USD" }
]
```

### **📌 Get Current Price of a Crypto Pair**

- **Endpoint:** `GET /api/CryptoPairs/price/{ticker}`
- **Description:** Retrieves the latest market price of the given cryptocurrency ticker.
- **How is the Current Price Determined?**
  - The price is retrieved from Tiingo's REST API.
  - The API response contains bid/ask data and last trade price.
  - The final **market price** is calculated as:
    ```csharp
    decimal marketPrice = (bidPrice + askPrice) / 2;
    ```
  - If bid/ask prices are not available, the **last trade price** is used.
- **Response:**

```json
{
  "ticker": "btcusd",
  "marketPrice": 94237.55
}
```

- **Possible Errors:**
  - `400 Bad Request`: Invalid ticker
  - `403 Forbidden`: API key is invalid or rate limit exceeded
  - `404 Not Found`: Price data not available
  - `500 Internal Server Error`: Unexpected failure

---

## **2️⃣ WebSocket Service**

### **📌 Subscribe to Live Crypto Price Updates**

- **WebSocket Endpoint:** `ws://localhost:5235/cryptoPriceHub`
- **Subscription Example:**

```javascript
const socket = new WebSocket("ws://localhost:5235/cryptoPriceHub");

socket.onopen = function () {
  console.log("Connected to WebSocket");
  socket.send(JSON.stringify({ action: "subscribe", ticker: "btcusd" }));
};

socket.onmessage = function (event) {
  console.log("Price Update:", event.data);
};
```

- **Incoming Messages:**

```json
{
  "ticker": "btcusd",
  "marketPrice": 94237.55
}
```

---

## **3️⃣ Data Source**

- The **Tiingo API** is used to fetch live price data.
- API Base URL: `https://api.tiingo.com/tiingo/crypto/prices`
- WebSocket Base URL: `wss://api.tiingo.com/crypto`

---

## **4️⃣ Performance, Logging, and Error Handling**

- The service efficiently handles **over 1,000 WebSocket subscribers** by maintaining **a single connection to Tiingo API**.
- Broadcasting is **optimized** so that **only relevant clients receive price updates**.
- The service logs **every critical event and error** to assist in debugging and monitoring.
- **Logging is implemented using `ILogger<T>`**
- Errors are handled gracefully with appropriate HTTP status codes.
- The WebSocket service logs **empty messages, errors, and successful subscriptions**.

---

## **5️⃣ Security and API Key Management**

- The API key is retrieved from the **environment variable (`TIINGO_API_KEY`)**.
- **For Docker deployment**, set the API key in `appsettings.json` instead of using an environment variable.

---

## **6️⃣ Future Enhancements**

- ✅ **Unit Tests**
- ✅ **Authentication using JWT for API security**
- ✅ **Advanced caching for real-time price improvements**
- ✅ **Rate-limiting to prevent abuse**

---
