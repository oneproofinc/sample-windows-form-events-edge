# ONEPROOF Edge USB Event Monitoring Windows Forms App

This sample Windows Forms application demonstrates how to integrate with ONEPROOF Edge devices using our native Windows DLL (`edge_windows.dll`). It provides a reference implementation for monitoring USB and BLE events from ONEPROOF Edge devices, with detailed event logging and data verification capabilities.

## What is ONEPROOF?

ONEPROOF provides comprehensive solutions for ISO 18013-5 and ISO 18013-7 compliant Mobile Driver's License (mDL) and National ID verification. Our product suite includes:

### Core Products
- **Mobile SDK**: Native libraries for iOS and Android to integrate mDL verification into mobile applications for ISO 18013-5 compliance
- **Server Deployments**: Scalable server solutions for enterprise-grade identity verification
- **APIs**: RESTful APIs for seamless integration with existing systems
- **Container Images**: Docker containers for easy deployment in cloud environments with ISO 18013-7 compliance
- **Edge Device**: Embedded hardware verification device for secure mDL and National ID scanning
- **Custom Solutions**: Tailored hardware and software solutions for specific use cases

### Key Features
- **ISO Compliance**: Full support for ISO 18013-5 and ISO 18013-7 standards
- **Multi-Platform Support**: Solutions for mobile, desktop, and server environments
- **Secure Verification**: End-to-end encrypted verification process
- **Custom Firmware**: Specialized firmware for specific verification requirements
- **Hardware Solutions**: Dedicated verification devices and custom hardware options
- **Integration Support**: Comprehensive documentation and developer tools

### Use Cases
- Mobile Driver's License verification
- National ID verification
- Age verification
- Identity authentication
- Banking & Finance
- Retail
- Access control
- Compliance reporting

Learn more about our complete product suite at [https://oneproof.com](https://oneproof.com)

## 🔧 Requirements

- Windows OS (x64)
- .NET 6.0 or later
- ONEPROOF Edge device
- Native DLLs: `edge_windows.dll` (contact ONEPROOF for access)

## 🚀 Getting Started (For Developers)

1. **Clone this repository**

2. **Build the project**:
   ```bash
   dotnet build
   ```

3. **Add the required DLLs**:
   - Place `edge_windows.dll` in the output directory
   - Default location: `bin\Debug\net6.0` or `bin\Release\net6.0`

4. **Run the sample app**:
   ```bash
   dotnet run
   ```

## 📁 Project Structure

- `Program.cs` – Main entry point for the Windows Forms application
- `Form1.cs` – Main form implementation with UI event handlers
- `Form1.Designer.cs` – Windows Forms designer-generated code
- `EdgeEvents.cs` – Core service class with P/Invoke bindings and USB monitoring logic

## 💻 Integration Guide

### 1. Create the Service Instance

```csharp
var service = new UsbEventService();
```

### 2. Start Monitoring

```csharp
service.StartMonitoring(
    json: "{ \"org.iso.18013.5.1\": { \"issuing_country\": true } }",
    onEvent: (type, msg) => {
        // Handle events here
        string eventName = service.GetEventTypeName(type);
        Console.WriteLine($"[{eventName}] {msg}");
    },
    verify: true,
    timeoutSeconds: 30
);
```

### 3. Handle Events

The service supports the following event types:

| Code | Event Name | Description |
|------|------------|-------------|
| 0 | INFO | General information messages |
| 1 | DEVICE CONNECTED | ONEPROOF Edge device connected |
| 2 | DEVICE DISCONNECTED | ONEPROOF Edge device disconnected |
| 3 | QRCODE SCANNED | QR code scanned from device |
| 4 | DEVICE HKDF KEY | Device key received |
| 5 | BLE CONNECTED | BLE connection established |
| 6 | BLE DISCONNECTED | BLE connection terminated |
| 7 | BLE SENDING | Data being sent via BLE |
| 8 | BLE RECEIVING | Data being received via BLE |
| 9 | DATA RECEIVED | Data successfully received |
| 10 | VERIFYING DATA | Data verification in progress |
| 11 | ERROR | Error occurred during operation |

## 🔌 Native Integration

The sample uses the following native methods from `edge_windows.dll`:

```csharp
[DllImport("edge_windows.dll")] 
public static extern void register_event_callback(UsbEventCallback callback);

[DllImport("edge_windows.dll")] 
public static extern void start_usb_monitoring(byte[] dataPtr, UIntPtr dataLen, bool verify);

[DllImport("edge_windows.dll")] 
public static extern void stop_usb_monitoring();

[DllImport("edge_windows.dll")] 
public static extern bool is_monitoring_active();
```

## 📝 Data Request Format

The JSON configuration specifies which data elements to request from the ONEPROOF Edge device:

```json
{
  "org.iso.18013.5.1": {
    "family_name": false,
    "given_name": false,
    "portrait": false,
    "issuing_country": true,
    "birth_date": false,
    "issuing_authority": true
  }
}
```

## 🔍 Verification Response Format

When verification is enabled (`verify: true`), the device returns a JSON response with verification results. Here's an example response:

```json
{
  "doc_type_matched": true,
  "validity_matched": false,
  "isomain_mso_verified": true,
  "cert_path_verified": false,
  "cert_cn_verified": false,
  "cert_stpv_verified": false,
  "cose_verified": true,
  "isomain_verified_values": "868268706f7274...",
  "isoaamva_mso_verified": false,
  "isoaamva_verified_values": "",
  "errors_during_verification": ""
}
```

### Response Fields
- `doc_type_matched`: Whether the document type matches expected doctype
- `validity_matched`: Whether the document is valid and not expired
- `isomain_mso_verified`: Whether the ISO MSO (Mobile Security Object) is verified
- `cert_path_verified`: Whether the certificate path is valid
- `cert_cn_verified`: Whether the Common Name in certificate is verified
- `cert_stpv_verified`: Whether the Subject Type Public Key verification passed
- `cose_verified`: Whether the CBOR Object Signing and Encryption (COSE) is verified
- `isomain_verified_values`: CBOR-encoded values from ISO verification (requires decoding)
- `isoaamva_mso_verified`: Whether the AAMVA MSO is verified
- `isoaamva_verified_values`: CBOR-encoded values from AAMVA verification (requires decoding)
- `errors_during_verification`: Any errors encountered during verification

### Non-Verification Mode

When verification is disabled (`verify: false`), the device will return encrypted data that you need to verify yourself. The process involves two key events:

1. **Device Key Event** (Code 4):
   - Event: `DEVICE HKDF KEY`
   - Description: Device key received
   - This key is required to decrypt the data

2. **Data Event** (Code 9):
   - Event: `DATA RECEIVED`
   - Description: Data successfully received
   - Contains the CBOR Encoded data

The CBOR Encoded data will be in one of these formats:

```cbor
{
  "data": "EncryptedData"
}
```

or

```cbor
{
  "data": "EncryptedData",
  "status": 20
}
```

You'll need to:
1. Store the device key when received (Event Code 4)
2. Use the key to decrypt the data when received (Event Code 9) based on ISO 18013-5.
3. Perform your own verification of the decrypted data including IACA Cert Validation.

### CBOR Decoding
The `isomain_verified_values` and `isoaamva_verified_values` fields contain CBOR-encoded data that needs to be decoded to access the actual values. You'll need to use a CBOR decoder library to process these fields.

Example using C#:
```csharp
using PeterO.Cbor;

// Decode CBOR data
var cborData = CBORObject.DecodeFromBytes(Convert.FromHexString(isomain_verified_values));
// Access decoded values
var decodedValues = cborData.ToJSONString();
```

## ⚠️ Important Notes

- Ensure `edge_windows.dll` is compiled for x64 architecture
- The ONEPROOF Edge device must be connected via USB
- The application uses Windows Forms for the user interface
- Proper error handling should be implemented in production code
- Contact ONEPROOF for licensing and support

## 📞 Support

For integration support or to obtain the required DLLs, please contact ONEPROOF technical support at [support@oneproof.com](mailto:support@oneproof.com).