using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace ChurchWebsite.Areas.Admin.Validation
{
    public sealed class ValidateFileAttribute : RequiredAttribute
    {
        private readonly int _width = 0;
        private readonly int _height = 0;

        public ValidateFileAttribute(int Width, int Height)
        {
            _width = Width;
            _height = Height;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                byte[] file = (byte[])value;
                //byte[] data = (byte[])dt.Rows[0]["IMAGE"];
                try
                {
                    MemoryStream ms = new MemoryStream(file);
                    var img = Image.FromStream(ms);
                    //var img = Image.FromStream(file.InputStream);
                    if (img.Width != _width || img.Height != _height)
                    {
                        return new ValidationResult("This picture isn't the right size!!!!");
                    }
                }
                catch (Exception ex)
                {
                    return new ValidationResult("This isn't a real image!");
                }


                if (file == null || file.Length > 1 * 1024 * 1024)
                {
                    return new ValidationResult("File size is too large please optimize your images!");
                }

                /*if (IsFileTypeValid(file))
                {
                    return new ValidationResult("This file type is not supported please use .PNG .JPEG or .GIF");
                }*/
            }

            return ValidationResult.Success;
        }

        private bool IsFileTypeValid(byte[] file)
        {
            bool isValid = false;

            try
            {
                MemoryStream ms = new MemoryStream(file);
                using (var img = Image.FromStream(ms))
                {
                    if (IsOneOfValidFormats(img.RawFormat))
                    {
                        isValid = true;
                    }
                }
            }
            catch
            {
                //Image is invalid
            }
            return isValid;
        }

        private bool IsOneOfValidFormats(ImageFormat rawFormat)
        {
            List<ImageFormat> formats = getValidFormats();

            foreach (ImageFormat format in formats)
            {
                if (rawFormat.Equals(format))
                {
                    return true;
                }
            }
            return false;
        }

        private List<ImageFormat> getValidFormats()
        {
            List<ImageFormat> formats = new List<ImageFormat>();
            formats.Add(ImageFormat.Png);
            formats.Add(ImageFormat.Jpeg);
            formats.Add(ImageFormat.Gif);
            //add types here
            return formats;
        }
    }

    public class FileUploadValidator : DataAnnotationsModelValidator<ValidateFileAttribute>
    {
        int requiredWidth = 0;
        int requiredHeight = 0;
        string errorMsg = string.Empty;

        public FileUploadValidator(ModelMetadata metadata, ControllerContext context, ValidateFileAttribute attribute)
            : base(metadata, context, attribute)
        {
            errorMsg = attribute.ErrorMessage;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            ModelClientValidationRule ValidationRule = new ModelClientValidationRule();
            ValidationRule.ErrorMessage = errorMsg;
            ValidationRule.ValidationType = "ValidateFile";
            ValidationRule.ValidationParameters.Add("Width", requiredWidth);
            ValidationRule.ValidationParameters.Add("Height", requiredHeight);
            return new[] { ValidationRule };
        }
    }
}