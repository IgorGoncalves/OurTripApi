using OurTripAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OurTripAPI.Controllers
{
    public class UserController : ApiController
    {
        OurtripEntities bd = new OurtripEntities();


        #region AddUser

        [Route("User/add/{name}/{dateborn}/{bio}/")]
        public User Post(int id, string name)
        {
            return bd.User.Find(id);
        }

        // POST: User/5/me
        [Route("User/{id}/me")]
        public User Post(int id)
        {
            return bd.User.Find(id);
        }

        #endregion

        #region AddLocation
        [Route("Location/add/{latitude}/{longitude}/{title}/{userId}")]
        public Location Get(double latitude, double longitude, string title, int userId)
        {
            string ponto = "POINT(" + longitude + " " + latitude + ")";
            DbGeography point = DbGeography.FromText(ponto.Replace(',', '.'));

            if (!(bd.User.ToList().Last(x => x.us_id == userId).Location.Last().Title == title))
            {

                Location location = new Location
                {
                    Title = title,
                    Point = point,
                    User = bd.User.ToList().Find(x => x.us_id == userId)
                };
                bd.LocationSet.Add(location);
                bd.SaveChanges();
            }
            return bd.LocationSet.ToList().Find(x => x.Point == point);
        }

        #endregion

        #region MeusDados

        // GET: User/5/me        
        [Route("User/{id}/me")]
        public User Get(int id)
        {
            return bd.User.Find(id);
        }

        //// POST: User/5/me
        //[Route("User/{id}/me")]
        //public User Post(int id)
        //{
        //    return bd.User.Find(id);
        //}

        #endregion

        #region UserLocations

        // GET: User/5/me        
        [Route("User/{id}/locations")]
        [HttpGet]
        public IEnumerable<Location> GetLocations(int id)
        {
            return bd.User.Find(id).Location;
        }

        // POST: User/5/me
        [Route("User/{id}/locations")]
        [HttpPost]
        public IEnumerable<Location> PostLocations(int id)
        {
            return bd.User.Find(id).Location;
        }


        #endregion

        #region Friends 

        // GET: api/friends
        [Route("api/friends/{latitude}/{longitude}/{distance}/{myId}/")]
        public IEnumerable<User> Get(float Latitude, float Longitude, float distance, int myId)
        {
            string ponto = "POINT(" + Longitude + " " + Latitude + ")";
            DbGeography point = DbGeography.FromText(ponto.Replace(',', '.'));
            return bd.User.ToList().FindAll(us => us.Location.Last().Point.Distance(point) < distance * 1000 && us.us_id != myId);
        }

        // POST: api/friends
        [Route("api/friends/{latitude}/{longitude}/{distance}/")]
        public IEnumerable<User> POST(float Latitude, float Longitude, float distance)
        {
            string ponto = "POINT(" + Latitude + " " + Longitude + ")";
            DbGeography point = DbGeography.FromText(ponto.Replace(',', '.'));
            return bd.User.ToList().FindAll(us => us.Location.Last().Point.Distance(point) < distance * 1000);
        }

        #endregion

        // DELETE: api/User/5
        public void Delete(int id)
        {
            bd.User.Remove(bd.User.ToList().Find(x => x.us_id == id));
        }
    }
}
