using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ChurchWebsite.Areas.Admin.Utilities.Web;
using ChurchWebsite.Models;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace ChurchWebsite.Areas.Admin.Controllers
{
    public class ImageController : BaseController
    {
        private ISiteAdminContext context = new SiteAdminContext();
        private const int placeholderId = 10;

        //
        // GET: /Image/

        public ActionResult ImageUploader()
        {
            return PartialView();
        }

        public ActionResult _ImageLib(int imgTyp = -1)
        {
            var myImageLibModel = GetLatestImageLibrary(imgTyp);
            return PartialView("_ImageLibrary", myImageLibModel);
        }

        private ImageLibraryModel GetLatestImageLibrary(int imageType)
        {
            ImageLibraryModel myReturnModel = new ImageLibraryModel();
            List<CImage> images;

            if (imageType == -1)
            {
                images = context.CImages.ToList();
            }
            else
            {
                images = (from img in context.CImages
                          where img.type == imageType
                          orderby img.createdDate descending
                          select img).ToList();
            }

            myReturnModel.displayMode = imageType;
            myReturnModel.displayImages = images;
            return myReturnModel;
        }

        public ActionResult _DeleteImageRequested(int imgId)
        {
            CImage image = context.FindPhotoById(imgId);
            if (image == null)
            {
                return HttpNotFound();
            }
            List<CEvent> eventsWithImage;
            eventsWithImage = (from evt in context.CEvents
                               where evt.imgId == imgId
                               select evt).ToList();

            return PartialView("_ImageDeleteConfirm", eventsWithImage);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult _DeleteConfirmed(int id)
        {
            CImage image = context.FindPhotoById(id);
            var myImageType = image.type;

            context.Delete<CImage>(image);
            context.SaveChanges();

            var myImageLibModel = GetLatestImageLibrary(myImageType);
            return PartialView("_ImageLibrary", myImageLibModel);
        }

        public FileContentResult GetImage(int id)
        {
            CImage photo = context.FindPhotoById(id);
            //Photo photo = context.Photos.Find(id);
            if (photo != null)
            {
                return File(photo.imageFile, photo.imageMimeType);
            }
            else
            {
                photo = context.FindPhotoById(placeholderId);
                return File(photo.imageFile, photo.imageMimeType);
            }
        }

        public FileContentResult GetPhotoThumbnail(int id, int width, int height)
        {
            CImage photo = context.FindPhotoById(id);
            if (photo != null)
            {
                MemoryStream ms = new MemoryStream(photo.imageFile);
                Image returnImage = Image.FromStream(ms);
                var thumb = resizeImage(returnImage, new Size(width, height), photo.imageMimeType);
                // Return byte array as jpeg.
                return File(thumb, photo.imageMimeType);
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult GetBase64Image(int id, int width, int height)
        {
            CImage photo = context.FindPhotoById(id);
            if (photo == null) photo = context.FindPhotoById(placeholderId);
            if (photo != null)
            {
                MemoryStream ms = new MemoryStream(photo.imageFile);
                Image returnImage = Image.FromStream(ms);
                byte[] thumb = resizeImage(returnImage, new Size(100, 205), photo.imageMimeType);
                ms.Read(thumb, 0, thumb.Length);

                return Json(new { base64imgage = Convert.ToBase64String(thumb) }
                    , JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        private byte[] resizeImage(Image imgToResize, Size size, string mimeType)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            using (var ms = new MemoryStream())
            {
                //utility here to get mime tpe
                b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }

            return null;
        }

        public ActionResult _ImageUploadForm(int imgTyp = -1)
        {
            CImage newImage = new CImage();
            newImage.createdDate = DateTime.Today;
            return PartialView("_ImageUploadForm", newImage);
        }

        [HttpPost]
        public WrappedJsonResult _ImageUploadForm(FormCollection myForm, HttpPostedFileWrapper imageFile)
        {
            if (imageFile == null || imageFile.ContentLength == 0)
            {
                return new WrappedJsonResult
                {
                    Data = new
                    {
                        IsValid = false,
                        Message = "No file was uploaded.",
                        ImagePath = string.Empty
                    }
                };
            }

            //------------------------------------My Stuff---------------------------------//
            CImage myUploadImage = new CImage();
            myUploadImage.title = myForm["title"];
            myUploadImage.description = myForm["description"];
            myUploadImage.createdDate = DateTime.Today;
            myUploadImage.type = 1;//(int)CImage.ImageType.Event;
            myUploadImage.userName = "tomh";
            myUploadImage.createdDate = DateTime.Now;
            myUploadImage.imageMimeType = imageFile.ContentType;
            myUploadImage.imageFile = new byte[imageFile.ContentLength];
            imageFile.InputStream.Read(myUploadImage.imageFile, 0, imageFile.ContentLength);

            try
            {
                context.Add<CImage>(myUploadImage);
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    //Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    //eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        //ve.PropertyName, ve.ErrorMessage);
                        ModelState.AddModelError(ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }

            //-------------------------------------------My Stuff ends-------------------------------------------//
            if (!ModelState.IsValid)
            {
                var myResult = new WrappedJsonResult
                {
                    Data = new
                    {
                        IsValid = false,
                        Message = RenderPartialViewToString("_ImageUploadForm", myUploadImage),
                        ImagePath = ""
                    }
                };
                return myResult;
            }

            var fileName = String.Format("{0}.jpg", Guid.NewGuid().ToString());
            /*var imagePath = Path.Combine(Server.MapPath(Url.Content("~/Uploads")), fileName);

            imageFile.SaveAs(imagePath);*/

            return new WrappedJsonResult
            {
                Data = new
                {
                    IsValid = true,
                    Message = "",
                    ImagePath = Url.Content(String.Format("~/Uploads/{0}", fileName))
                }
            };
        }
    }
}
