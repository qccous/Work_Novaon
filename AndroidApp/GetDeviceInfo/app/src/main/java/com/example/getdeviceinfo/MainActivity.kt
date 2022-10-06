package com.example.getdeviceinfo

import android.content.Context
import android.content.pm.PackageManager
import android.hardware.usb.UsbDevice.getDeviceId
import android.net.wifi.WifiManager
import android.os.Build
import android.os.Bundle
import android.support.annotation.RequiresApi
import android.support.v4.app.ActivityCompat
import android.support.v7.app.AppCompatActivity
import android.telephony.TelephonyManager
import android.text.format.Formatter.formatIpAddress
import android.widget.TextView
import android.widget.Toast
import java.util.*


class MainActivity : AppCompatActivity() {


    @RequiresApi(Build.VERSION_CODES.O)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        // val textView: TextView = findViewById(R.id.tvMsg) as TextView
        val result = findViewById<TextView>(R.id.tvMsg);
        var imei: String? = GetIMEI();
        var mac: String? = GetIP();
        var output: String =
            ("Model: " + Build.MODEL + "\n" + imei + "\n" + Build.VERSION.SDK_INT + "\n" + mac);
        result.setText(output).toString();
    }

    @RequiresApi(Build.VERSION_CODES.O)
    private fun GetIMEI(): String? {
        var myIMEI: String? = null
        try {
            val tm = getSystemService(Context.TELEPHONY_SERVICE) as TelephonyManager
            if (ActivityCompat.checkSelfPermission(
                    this,
                    android.Manifest.permission.READ_PHONE_STATE
                ) != PackageManager.PERMISSION_GRANTED
            ) {
                return null;
            }
            val IMEI = android.telephony.TelephonyManager.getDeviceId()
            if (IMEI != null) {
                myIMEI = IMEI;
            }
        } catch (ex: Exception) {
            Toast.makeText(this, ex.toString(), Toast.LENGTH_SHORT).show()
        }
        return myIMEI
    }

    private fun GetIP(): String {

        val wifiManager = applicationContext.getSystemService(Context.WIFI_SERVICE) as WifiManager
       // val ipAddress = Formatter.formatIpAddress(wifiManager.getConnectionInfo().getIpAddress())
        val mac = wifiManager.connectionInfo.macAddress
        val result: String = mac.toString()
        return result
    }
}