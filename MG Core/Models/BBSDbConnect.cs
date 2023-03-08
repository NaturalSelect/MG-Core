using System;
using System.Collections.Generic;
using System.Linq;
using MG_Core.Data;
using System.Threading.Tasks;
using System.Text.Encodings.Web;

namespace MG_Core.Models
{
    public class BBSDbConnect
    {
        private readonly ApplicationDbContext Context;
        public BBSDbConnect(ApplicationDbContext context)
        {
            Context = context;
        }
        public Block FindBlockById(string Id)
        {
            var q = from c in Context.Block
                    where c.Id == Id
                    join u in Context.Users
                    on c.Id equals u.Block.Id
                    select new Block() { Id = c.Id, ImgPath = c.ImgPath, Name = c.Name, PowerUser = u };
            if(q.Count() == 0)
            {
                return null;
            }
            var b = q.First();
            b.Post=GetBlockAllPost(b.Id);
            return b;
        }
        public Block FindBlockByName(string Name)
        {
            var q =from c in Context.Block
                   where c.Name == Name
                   join u in Context.Users
                    on c.Id equals u.Block.Id
                   select new Block() { Id = c.Id, ImgPath = c.ImgPath, Name = c.Name, PowerUser = u };
            if(q.Count() == 0)
            {
                return null;
            }
            var b = q.First();
            b.Post = GetBlockAllPost(b.Id);
            return b;
        }
        public Post[] GetBlockAllPost(string Id)
        {
            var q = from c in Context.Block
                    where c.Id == Id
                    join p in Context.Post
                    on c.Id equals p.Block.Id
                    select p;
            return q.OrderByDescending(x=>x.Time).ToArray();
        }
        public bool HasSomeName(string Name,string Id)
        {
            var b = FindBlockByName(Name);
            if(b != null && b.Id != Id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<string> CreateBlock(Block block)
        {
            if (FindBlockByName(block.Name)!=null)
            {
                return "二义性操作，具有相同的名称";
            }
            Context.Add(block);
            await Context.SaveChangesAsync();
            return null;
        }
        public async Task<string> EditBlock(Block block)
        {
            var b = FindBlockById(block.Id);
            if(b == null)
            {
                return "找不到Block";
            }
            b.Name = block.Name;
            if (!string.IsNullOrEmpty(block.ImgPath))
            {
                b.ImgPath = block.ImgPath;
            }
            else
            {
                b.ImgPath = b.ImgPath;
            }
            b.Introduction = block.Introduction;
            await RemoveUserBlock(block.PowerUser.Id);
            b.PowerUser = block.PowerUser;
            Context.Update(b);
            await Context.SaveChangesAsync();
            return null;
        }
        public async Task RemoveUserBlock(string Id)
        {
            var q = from c in Context.Block
                    join u in Context.Users.Where(x=>x.Id == Id)
                    on c.Id equals u.Block.Id
                    select u;
            if(q.Count() == 0)
            {
                return;
            }
            var user = q.First();
            user.Block = null;
            Context.Users.Update(user);
            await Context.SaveChangesAsync();
        }
        public async Task<string> AddPostToBlock(Post post,string BlockId)
        {
            var b = FindBlockById(BlockId);
            
            if(b == null)
            {
                return "找不到Block";
            }
            var p = GetBlockAllPost(BlockId);
            b.Post = p.ToList();
            b.Post.Add(post);
            Context.Block.Update(b);
            await Context.SaveChangesAsync();
            return null;
        }
        public Post FindPostById(int PostId)
        {
            var q = from c in Context.Post
                    where c.Id == PostId
                    select c;
            if(q.Count() == 0)
            {
                return null;
            }
            var p = q.First();
            p.User = GetPostUser(p.Id);
            p.Block = FindBlockByPostId(p.Id);
            p.Reply = GetReplyFromPost(p.Id);
            return p;

        }
        public Block FindBlockByPostId(int PostId)
        {
            var q = from c in Context.Block
                    join p in Context.Post
                    on c.Id equals p.Block.Id
                    select c;
            if(q.Count() == 0)
            {
                return null;
            }
            return q.First();
        }
        public ApplicationUser GetPostUser(int PostId)
        {
            var q = from c in Context.Users
                    join p in Context.Post.Where(x => x.Id == PostId)
                    on c.Id equals p.User.Id
                    select c;
            if(q.Count() == 0)
            {
                return null;
            }
            return q.First();
        }
        public async Task<string> RemovePostFormBlock(int PostId,string BlockId)
        {
            var b = FindBlockById(BlockId);
            if(b == null)
            {
                return "找不到Block";
            }
            var p = GetBlockAllPost(BlockId);
            if(p.Count() == 0)
            {
                return "该Block无帖子";
            }
            b.Post = p;
            b.Post.Remove(FindPostById(PostId));
            Context.Block.Update(b);
            await Context.SaveChangesAsync();
            return null;
        }
        public Reply[] GetReplyFromPost(int PostId)
        {
            var q = from c in Context.Post
                    where c.Id == PostId
                    join r in Context.Reply
                    on c.Id equals r.Post.Id
                    orderby r.Time
                    select r;
            if(q.Count() == 0)
            {
                return null;
            }
            foreach(var re in q)
            {
                re.UserName = GetReplyUser(re.Id);
            }
            return q.OrderBy(x=>x.Time).ToArray();
        }
        public ApplicationUser GetReplyUser(string ReplyId)
        {
            var q = from c in Context.Users
                    join r in Context.Reply.Where(x=>x.Id == ReplyId)
                    on c.Id equals r.UserName.Id
                    select c;
            if(q.Count() == 0)
            {
                return null;
            }
            return q.First();
        }
        public async Task<string> AddReplyToPost(Reply reply,int PostId)
        {
            var p = FindPostById(PostId);
            if(p == null)
            {
                return "找不到帖子";
            }
            if(p.Reply == null)
            {
                p.Reply = new List<Reply>();
            }
            var r = p.Reply.ToList();
            r.Add(reply);
            p.Reply = r.ToArray();
            Context.Post.Update(p);
            await Context.SaveChangesAsync();
            return null;
        }
        public Block[] GetBlockList()
        {
            var q = from c in Context.Block
                    join u in Context.Users
                    on c.Id equals u.Block.Id
                    select new Block() { Id = c.Id, ImgPath = c.ImgPath, Name = c.Name, PowerUser = u,Introduction = c.Introduction };
            if(q.Count() == 0)
            {
                return null;
            }
            return q.ToArray();
        }
        public async Task RemoveBlock(string BlockId)
        {
            Context.Block.Remove(FindBlockById(BlockId));
            await Context.SaveChangesAsync();
        }
        public ApplicationUser GetBlockPowerUser(string BlockId)
        {
            var q = from c in Context.Block
                    where c.Id == BlockId
                    join u in Context.Users
                    on c.Id equals u.Block.Id
                    select u;
            if(q.Count() == 0)
            {
                return null;
            }
            return q.First();
        }
        public bool IsBlockPowerUser(ApplicationUser user,string BlockId)
        {
            var u = GetBlockPowerUser(BlockId);
            if(user.Id == u.Id)
            {
                return true;
            }
            return false;
        }
        public Post[] GetNEWS()
        {
            var q = from u in Context.Users
                    join p in Context.Post.OrderByDescending(x=>x.Time).Take(5)
                    on u.Id equals p.User.Id
                    select new Post() { Title = p.Title, Id = p.Id, User = u };
            return q.ToArray();
        }
        
    }
}
