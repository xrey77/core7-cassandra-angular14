using Cassandra;
using Cassandra.Mapping;
using core7_cassandra_angular14.Entities;

namespace core7_cassandra_angular14.Services
{
    public interface IAuthService {
        Boolean SignUp(User user);
        User SignUser(string usrname);
    }

    public class AuthService : IAuthService
    {
        private Cluster _cluster;
        private IMapper _mapper;
        private Cassandra.ISession _session;

        public AuthService() {
            _cluster = Cluster.Builder().AddContactPoint("127.0.0.1").WithPort(9042).Build();
            _session = _cluster.Connect("core7");
            _mapper = new Mapper(_session);
        }


        private bool findEmail(string xmail) {
          try {
            User usermail = _mapper.Single<User>("SELECT email FROM users WHERE email = ? ALLOW FILTERING", xmail);
            if (usermail is not null) {
                return true;
            }
          } catch(Exception) {}
          return false;
        }

        private bool findUsername(string uname) {
            try {
                User usermail = _mapper.Single<User>("SELECT username FROM users WHERE username = ? ALLOW FILTERING", uname);
                if (usermail is not null) {
                    usermail.Email = null;
                    return true;
                }
                return false;
            } catch(Exception) {} 
            return false;
        }

        private int NextId() {
            var cnt = _mapper.Single<User>("SELECT count(id) as Id FROM users");
            return cnt.Id;
        }

        public bool SignUp(User user)
        {
            if (findEmail(user.Email) == true) {
                throw new Exception("Email Address is already taken....");
            }
            if (findUsername(user.Username) == true)  {
                throw new Exception("User Name is already taken....");
            }
            int ln = NextId();
            var adduser = _session.Prepare("INSERT INTO users(id,firstname,lastname,email,mobile,username,pasword,secretkey,profilepic,created_at) VALUES(?,?,?,?,?,?,?,?,?,toTimestamp(now()))");
            var batch = new BatchStatement()
            .Add(adduser.Bind(ln ,user.Firstname,user.Lastname,user.Email,user.Mobile,user.Username,user.Pasword, user.Secretkey, user.Profilepic));
            _session.ExecuteAsync(batch);
            return true;
        }

        public User SignUser(string usrname)
        {
            try {
                User user = _mapper.Single<User>("SELECT * FROM users WHERE username = ? ALLOW FILTERING" , usrname);
                return user;
            } catch(Exception ex) {                
                throw new Exception(ex.Message);
            } 
        }
    }

}