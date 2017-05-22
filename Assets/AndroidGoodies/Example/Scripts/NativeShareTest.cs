#if UNITY_ANDROID
using UnityEngine;
using DeadMosquito.AndroidGoodies;
using System.Collections;
using System.IO;

namespace AndroidGoodiesExamples
{
    public class NativeShareTest : MonoBehaviour
    {
        public string message = "Android Native Goodies PRO by Dead Mosquito Games http://u3d.as/xf8 #AssetStore";
        public Texture2D image;

        public bool withChooser = true;

        public string subject;
        public string text;

        public void OnShareClick()
        {
            AGShare.ShareTextWithImage(subject, text, image);
        }

        public void OnSendEmailClick()
        {
            var recipients = new[] {"x@gmail.com", "hello@gmail.com"};
            var ccRecipients = new[] {"cc@gmail.com"};
            var bccRecipients = new[] {"bcc@gmail.com", "bcc-guys@gmail.com"};
            AGShare.SendEmail(recipients, "subj", "body", image, withChooser, cc: ccRecipients, bcc: bccRecipients);
        }

        public void OnSendSmsClick()
        {
            AGShare.SendSms("123123123", "Hello", withChooser);
        }

        public void OnSendMmsClick()
        {
            AGShare.SendMms("777-888-999", "Hello my friend", image, false, "MMS!");
        }

        public void OnTweetClick()
        {
            AGShare.Tweet(message, image);
        }

        public void OnShareScreenshot()
        {
            AGShare.ShareScreenshot();
        }

        // FB Messenger
        public void OnSendFacebookMessageText()
        {
            AGShare.SendFacebookMessageText(message);
        }

        public void OnSendFacebookMessageImage()
        {
            AGShare.SendFacebookMessageImage(image);
        }

        // WhatsApp
        public void OnSendWhatsAppTextMessage()
        {
            AGShare.SendWhatsAppTextMessage(message);
        }

        public void OnSendWhatsAppImageMessage()
        {
            AGShare.SendWhatsAppImageMessage(image);
        }

        // Instagram
        public void OnSendInstagramImage()
        {
            AGShare.ShareInstagramPhoto(image);
        }

        // Telegram
        public void OnSendTelegramTextMessage()
        {
            AGShare.SendTelegramTextMessage(message);
        }

        public void OnSendTelegramImageMessage()
        {
            AGShare.SendTelegramImageMessage(image);
        }

        // Viber
        public void OnSendViberTextMessage()
        {
            AGShare.SendViberTextMessage(message);
        }

        public void OnSendViberImageMessage()
        {
            AGShare.SendViberImageMessage(image);
        }

        // SnapChat
        public void OnSendSnapChatTextMessage()
        {
            AGShare.SendSnapChatTextMessage(message);
        }

        public void OnSendSnapChatImageMessage()
        {
            AGShare.SendSnapChatImageMessage(image);
        }

        public void OnShareVideo()
        {
            // NOTE: In order to test this method video file must exist on file system.
            // To test this method you put 'xxx.mov' video into your downloads folder
            const string videoFileName = "/xxx.mov";
            var filePath = Path.Combine(AGEnvironment.ExternalStorageDirectoryPath, AGEnvironment.DirectoryDownloads) + videoFileName;
            Debug.Log("Sharing video: " + filePath + ", file exists?: " + File.Exists(filePath));
            AGShare.ShareVideo(filePath, "My Video Title", "Upload Video");
        }
    }
}
#endif