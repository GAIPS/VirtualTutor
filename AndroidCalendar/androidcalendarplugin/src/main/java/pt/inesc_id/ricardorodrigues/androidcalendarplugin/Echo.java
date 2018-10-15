package pt.inesc_id.ricardorodrigues.androidcalendarplugin;

import android.util.Log;

public class Echo {
    public static String Shout(String message) {
        Log.v("SHOUT", message);
        return "He shouted: " + message;
    }
}
