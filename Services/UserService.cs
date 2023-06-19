using System.Linq;
using Cassandra;
using Cassandra.Mapping;
using core7_cassandra_angular14.Entities;

namespace core7_cassandra_angular14.Services
{
    public interface IUserService {
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Update(User user);
        void Delete(int id);
        void ActivateMfa(int id, bool opt, string qrcode_url);
        void UpdatePicture(int id, string file);
    }

    public class UserService : IUserService
    {
        private Cluster _cluster;
        private IMapper _mapper;
        private Cassandra.ISession _session;

        public UserService() {
            _cluster = Cluster.Builder()
            .AddContactPoint("127.0.0.1")
            // .WithCredentials("rey","rey")
            .WithPort(9042).Build();
            _session = _cluster.Connect("core7");
            _mapper = new Mapper(_session);
        }

        public void ActivateMfa(int id, bool opt, string qrcode_url)
        {
            try {                
                if (opt == true) {
                    var uid = _session.Prepare("UPDATE users SET qrcodeurl = ? WHERE id = ?");
                    var batch = new BatchStatement().Add(uid.Bind(qrcode_url, id));
                    _session.ExecuteAsync(batch);

                } else {
                    var uid = _session.Prepare("UPDATE users SET qrcodeurl = ? WHERE id = ?");
                    var batch = new BatchStatement().Add(uid.Bind(null, id));
                    _session.ExecuteAsync(batch);
                }

            } catch(Exception ex) {
                throw new Exception(ex.Message);                
            }
        }

        public void Delete(int id)
        {
            try {                
                var uid = _session.Prepare("DELETE FROM users WHERE id = ?");
                var batch = new BatchStatement().Add(uid.Bind(id));
                _session.ExecuteAsync(batch);
            } catch(Exception ex) {
                throw new Exception(ex.Message);                
            }
        }

        public IEnumerable<User> GetAll()
        {
            try {
                var rs = _mapper.Fetch<User>("SELECT * FROM users");
                return rs;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            } 
        }

        public User GetById(int id)
        {
            User user = _mapper.Single<User>("SELECT * FROM users WHERE id = ? ALLOW FILTERING", id);
            if (user is null) {
                throw new Exception("User does'not exists....");
            }
            return user;
        }

        public void Update(User userParam)
        {
            User user = _mapper.Single<User>("SELECT * FROM users WHERE id = ? ALLOW FILTERING", userParam.Id);
            if (user == null)
                throw new Exception("User not found");

            if (!string.IsNullOrWhiteSpace(userParam.Firstname)) {
                user.Firstname = userParam.Firstname;
            }

            if (!string.IsNullOrWhiteSpace(userParam.Lastname)) {
                user.Lastname = userParam.Lastname;
            }

            if (!string.IsNullOrWhiteSpace(userParam.Mobile)) {
                user.Mobile = userParam.Mobile;
            }

            if (!string.IsNullOrWhiteSpace(userParam.Pasword))
            {
                 user.Pasword = BCrypt.Net.BCrypt.HashPassword(userParam.Pasword);
            }
            
            var timestamp = new DateTimeOffset(DateTime.UtcNow); //.ToUnixTimeSeconds();
            string unixTimestamp = Convert.ToString((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

            if (userParam.Pasword is not null) {
                var userStmt = _session.Prepare("UPDATE users SET firstname = ?, lastname = ?, mobile = ?, pasword = ?, updated_at = toTimestamp(now()) WHERE id = ?");
                var batch = new BatchStatement().Add(userStmt.Bind(
                    user.Firstname, user.Lastname, user.Mobile, user.Pasword, user.Id));
                _session.Execute(batch);
            } else {
                var userStmt = _session.Prepare("UPDATE users SET firstname = ?, lastname = ?, mobile = ?, updated_at = toTimestamp(now()) WHERE id = ?");
                var batch = new BatchStatement().Add(userStmt.Bind(
                    user.Firstname, user.Lastname, user.Mobile, user.Id));
                _session.Execute(batch);
            }
        }

        public void UpdatePicture(int id, string file)
        {
            try {
                var uploadStmt = _session.Prepare("UPDATE users SET profilepic = ?, updated_at = toTimestamp(now()) WHERE id = ?");
                var batch = new BatchStatement().Add(uploadStmt.Bind(file, id));
                _session.Execute(batch);
            } catch(Exception ex) {
               throw new Exception(ex.Message);
            }
        }
    }
}