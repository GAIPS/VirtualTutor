package pt.inesc_id.ricardorodrigues.androidcalendarplugin;

import android.Manifest;
import android.content.Context;
import android.content.pm.PackageManager;
import android.support.v4.content.ContextCompat;

public class CalendarPlugin {

    private Context context;

    public void SetContext(Context context) {
        this.context = context;
    }

    public boolean AskPermissions() {
        if (context == null) return false;

        if (ContextCompat.checkSelfPermission(context, Manifest.permission.READ_CALENDAR)
                != PackageManager.PERMISSION_GRANTED ||
                ContextCompat.checkSelfPermission(context, Manifest.permission.WRITE_CALENDAR)
                != PackageManager.PERMISSION_GRANTED) {
            return false;
        }
        return true;
    }
}
