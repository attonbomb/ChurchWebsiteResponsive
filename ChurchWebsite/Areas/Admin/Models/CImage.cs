using System;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ChurchWebsite.Areas.Admin.Utilities.Data;
using ChurchWebsite.Areas.Admin.Validation;
using System.Web.Helpers;

namespace ChurchWebsite.Models
{
    public class CImageMetaData
    {
        [HiddenInput(DisplayValue = false)]
        public string imageMimeType { get; set; }

        [ValidateFile(290, 500)]
        public byte[] imageFile { get; set; }
    }

    [MetadataType(typeof(CImageMetaData))]
    public partial class CImage
    {
        public enum ImageType
        {
            Banner = 0,
            Event = 1
        }

        public CImage DeepCopy()
        {
            CImage imgClone = (CImage)CDataUtilities.ShallowCopyEntity(this);
            return imgClone;
        }
    }
}