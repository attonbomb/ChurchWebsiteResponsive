using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ChurchWebsite.Models
{
    public class SiteAdminContext:NTCGLeedsEntities1, ISiteAdminContext
    {
        IQueryable<CEvent> ISiteAdminContext.CEvents
        {
            get { return CEvents; }
        }

        IQueryable<CImage> ISiteAdminContext.CImages
        {
            get { return CImages; }
        }

        int ISiteAdminContext.SaveChanges()
        {
            try
            {
                return SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        T ISiteAdminContext.Add<T>(T entity)
        {
            return Set<T>().Add(entity);
        }

        T ISiteAdminContext.Delete<T>(T entity)
        {
            return Set<T>().Remove(entity);
        }

        void ISiteAdminContext.Update(CEvent entity1, CEvent entity2)
        {
            Entry(entity1).CurrentValues.SetValues(entity2);
        }

        CImage ISiteAdminContext.FindPhotoById(int ID)
        {
            return Set<CImage>().Find(ID);
        }

        CEvent ISiteAdminContext.FindEventById(int ID)
        {
            return Set<CEvent>().Find(ID);
        }
    }
}