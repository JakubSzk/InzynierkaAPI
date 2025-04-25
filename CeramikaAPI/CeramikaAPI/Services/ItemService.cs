using CeramikaAPI.Context;
using CeramikaAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace CeramikaAPI.Services
{
    public class ItemService
    {
        private CeramikaContext context;
        public ItemService() { context = new CeramikaContext(); }
        private UserService userService = new UserService();
        public List<ItemListModelDTO> ItemsByFilter(float minPrice, float maxPrice, string author, string type, string[] tags)
        {
          
            List<ItemListModelDTO> hold = context.ItemTags.Where(c => (tags.Contains(c.Tag.Name) || tags.Contains(null))  &&
            (c.Item.Type == type || type=="") &&
            c.Item.Avaible > 0 &&
            c.Item.Price >= minPrice &&
            c.Item.Price <= maxPrice &&
            (c.Item.Author == author || author==""))
                .GroupBy(c => c.Item)
                .Select(g => new ItemListModelDTO
                {
                    Id = g.Key.Id,
                    Name = g.Key.Name,
                    Type = g.Key.Type,
                    Model = "",
                    Tags = context.ItemTags.Where(x => x.Item.Id == g.Key.Id).Select(y => y.Tag.Name).ToList(), //g.Select(t => t.Tag.Name).ToList(),
                    Author = g.Key.Author,
                    Price = g.Key.Price,
                    Description = "",
                    Photos = context.ItemPhotos.Where(x => x.Item.Id == g.Key.Id).Select(y => y.Photo.Name).ToList(),
                    Avaible = g.Key.Avaible
                }).ToList();
            return hold;
        }

        public ItemListModelDTO ItemData(int id)
        {
            ItemListModelDTO returnable = context.Items.Where(c => c.Id == id)
                .Select(g => new ItemListModelDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    Type = g.Type,
                    Model = g.Model,
                    Tags = context.ItemTags.Where(x => x.Item.Id == g.Id).Select(y => y.Tag.Name).ToList(), //g.Select(t => t.Tag.Name).ToList(),
                    Author = g.Author,
                    Price = g.Price,
                    Description = g.Description,
                    Photos = context.ItemPhotos.Where(x => x.Item.Id == g.Id).Select(y => y.Photo.Name).ToList(),
                    Avaible = g.Avaible
                }).ToList()[0];

            return returnable;

                
        }

        public List<String> GetTags()
        {
            List<String> returnable = context.Tags.Select(x => x.Name).ToList();
            return returnable;
        }

        public List<String> GetAuthors()
        {
            List<String> returnable = context.Items.Select(c => c.Author).Distinct().ToList();
            return returnable;
        }
        public List<String> GetTypes()
        {
            List<String> returnable = context.Items.Select(c => c.Type).Distinct().ToList();
            return returnable;
        }

        public bool AddTag(string name, string token)
        {
            bool? test = userService.VerifyUser(token);
            if (test == null || test == false) { return false; }
            context.Tags.Add(new TagModel { Name = name });
            try { context.SaveChanges(); }
            catch (Exception ex) { return false; }
            return true;
        }

        public bool AddTagsToItem(int idItem,string token, int[] idTags)
        {
            bool? test = userService.VerifyUser(token);
            if (test == null || test == false) { return false; }
            var hold = context.Items.First(x => x.Id == idItem);
            foreach (var idTag in idTags)
            {
                context.ItemTags.Add(new ItemTagModel { Item = hold, Tag = context.Tags.First(f => f.Id == idTag) });
            };
            try { context.SaveChanges(); }
            catch (Exception ex) { return false; }
            return true;
        }

        public bool AddPhotosToItem(int idItem, string token, int[] idPhotos)
        {
            bool? test = userService.VerifyUser(token);
            if (test == null || test == false) { return false; }
            var hold = context.Items.First(x => x.Id == idItem);
            foreach (var idPhoto in idPhotos)
            {
                context.ItemPhotos.Add(new ItemPhotoModel { Item = hold, Photo = context.Photos.First(f => f.Id == idPhoto) });
            }
            try { context.SaveChanges(); }
            catch (Exception ex) { return false; }
            return true;
        }

        public bool AddPhoto(string name, string token)
        {
            bool? test = userService.VerifyUser(token);
            if (test == null || test == false) { return false; }
            context.Photos.Add(new PhotoModel { Name = name });
            try { context.SaveChanges(); }
            catch (Exception ex) { return false; }
            return true;
        }

        public bool RemoveTag(string name, string token)
        {
            bool? test = userService.VerifyUser(token);
            if (test == null || test == false) { return false; }
            context.Tags.Remove(context.Tags.First(t =>  t.Name == name));
            try { context.SaveChanges(); }
            catch (Exception ex) { return false; }
            return true;
        }

        public bool RemovePhoto(string name, string token)
        {
            bool? test = userService.VerifyUser(token);
            if (test == null || test == false) { return false; }
            context.Photos.Remove(context.Photos.First(t => t.Name == name));
            try { context.SaveChanges(); }
            catch (Exception ex) { return false; }
            return true;
        }

        public bool AddItem(string name, string description, string type, int amount, float price, string model, string author, string token)
        {
            bool? test = userService.VerifyUser(token);
            if (test == null || test == false) { return false; }
            var hold = context.Items.Add(new ItemModel { Name = name, Description = description, Type = type, Avaible = amount, Price = price, Model = model, Author = author });
            
            
            try { context.SaveChanges(); } catch (Exception ex) { return false; }
            return true;

        }

        public List<PhotoModel> ShowPhotos()
        {
            return context.Photos.ToList();
        }

        public List<TagModel> ShowTags()
        {
            return context.Tags.ToList();
        }





    }
}
