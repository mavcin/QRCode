using Microsoft.AspNetCore.Mvc;
using QRCodeInASPNetCore.Models;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static QRCoder.PayloadGenerator;

namespace QRCodeInASPNetCore.Controllers
{
    public class QRCodeController : Controller
    {
        public IActionResult Index()
        {
            QRCodeModel model = new QRCodeModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(QRCodeModel model)
        {

            Payload payload = null;

            switch (model.QRCodeType)
            {
                case 1: // website url
                    payload = new Url(model.WebsiteURL);
                    break;
                case 2: // bookmark url
                    payload = new Bookmark(model.BookmarkURL, model.BookmarkURL);
                    break;
                case 3: // compose sms
                    payload = new SMS(model.SMSPhoneNumber, model.SMSBody);
                    break;
                case 4: // compose whatsapp message
                    payload = new WhatsAppMessage(model.WhatsAppNumber, model.WhatsAppMessage);
                    break;
                case 5://compose email
                    payload = new Mail(model.ReceiverEmailAddress, model.EmailSubject, model.EmailMessage);
                    break;
                case 6: // wifi qr code
                    payload = new WiFi(model.WIFIName, model.WIFIPassword, WiFi.Authentication.WPA);
                    break;

            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeAsBitmap = qrCode.GetGraphic(20);

            // use this when you want to show your logo in middle of QR Code and change color of qr code
            //Bitmap logoImage = new Bitmap(@"wwwroot/img/Virat-Kohli.jpg");
            //var qrCodeAsBitmap = qrCode.GetGraphic(20, Color.Black, Color.Red, logoImage);

            string base64String = Convert.ToBase64String(BitmapToByteArray(qrCodeAsBitmap));
            model.QRImageURL = "data:image/png;base64," + base64String;
            return View("Index", model);
        }
		
		public IActionResult Test()
        {

            return View();
        }

        public bool BosMu(string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        [HttpPost]
        public IActionResult Test(QRCodeModel model)
        {

            Payload payload = null;


            if(!BosMu(model.WebsiteURL))
            {
                if(model.WebsiteURL.ToString() != null)
                {
                    payload = new Url(model.WebsiteURL);
                }
               
            }
            if (!BosMu(model.BookmarkURL))
            {
                if(model.BookmarkURL.ToString() != null)
                {
                    payload = new Bookmark(model.BookmarkURL, model.BookmarkURL);
                }
                
            }
            if (!BosMu(model.SMSPhoneNumber) && !BosMu(model.SMSBody))
            {
                if(model.SMSPhoneNumber.ToString() != null && model.SMSBody.ToString() != null)
                {
                    payload = new SMS(model.SMSPhoneNumber, model.SMSBody);
                }
               
            }
            if (!BosMu(model.WhatsAppNumber) && !BosMu(model.WhatsAppMessage))
            {
                if(model.WhatsAppNumber.ToString() != null && model.WhatsAppMessage.ToString() != null)
                {
                    payload = new WhatsAppMessage(model.WhatsAppNumber, model.WhatsAppMessage);
                }
                
            }
            if (!BosMu(model.ReceiverEmailAddress) && !BosMu(model.EmailSubject) && !BosMu(model.EmailMessage))
            {
                if(model.ReceiverEmailAddress.ToString() != null && model.EmailSubject.ToString() != null && model.EmailMessage != null)
                {
                    payload = new Mail(model.ReceiverEmailAddress, model.EmailSubject, model.EmailMessage);
                }
                
            }
            if (!BosMu(model.WIFIName) && !BosMu(model.WIFIPassword))
            {
                if(model.WIFIName.ToString() != null && model.WIFIPassword.ToString() != null)
                {
                    payload = new WiFi(model.WIFIName, model.WIFIPassword, WiFi.Authentication.WPA);
                }
               
            }

            //switch (model.QRCodeType)
            //{
            //    case 1: // website url
            //        payload = new Url(model.WebsiteURL);
            //        break;
            //    case 2: // bookmark url
            //        payload = new Bookmark(model.BookmarkURL, model.BookmarkURL);
            //        break;
            //    case 3: // compose sms
            //        payload = new SMS(model.SMSPhoneNumber, model.SMSBody);
            //        break;
            //    case 4: // compose whatsapp message
            //        payload = new WhatsAppMessage(model.WhatsAppNumber, model.WhatsAppMessage);
            //        break;
            //    case 5://compose email
            //        payload = new Mail(model.ReceiverEmailAddress, model.EmailSubject, model.EmailMessage);
            //        break;
            //    case 6: // wifi qr code
            //        payload = new WiFi(model.WIFIName, model.WIFIPassword, WiFi.Authentication.WPA);
            //        break;

            //}

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeAsBitmap = qrCode.GetGraphic(20);

            // use this when you want to show your logo in middle of QR Code and change color of qr code
            //Bitmap logoImage = new Bitmap(@"wwwroot/img/Virat-Kohli.jpg");
            //var qrCodeAsBitmap = qrCode.GetGraphic(20, Color.Black, Color.Red, logoImage);

            string base64String = Convert.ToBase64String(BitmapToByteArray(qrCodeAsBitmap));
            model.QRImageURL = "data:image/png;base64," + base64String;
            return View("Test", model);
        }

        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
