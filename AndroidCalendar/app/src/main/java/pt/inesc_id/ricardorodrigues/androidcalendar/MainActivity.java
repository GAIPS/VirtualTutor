package pt.inesc_id.ricardorodrigues.androidcalendar;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;

import pt.inesc_id.ricardorodrigues.androidcalendarplugin.Echo;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.v("MyLog", Echo.Shout("ALELUIA"));
        setContentView(R.layout.activity_main);
    }
}
