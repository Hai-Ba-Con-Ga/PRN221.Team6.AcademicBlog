using AcademicBlog.BussinessObject;
using Azure.Identity;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;

namespace AcademicBlog.Utils
{
    public static class Utils
    {
        public static string GetRandomPastelColor()
        {
            var pastelColors = new List<string>
    {
        "bg-gray-50  text-gray-600 ring-gray-500/10",
        "bg-red-50 text-red-700 ring-red-600/10",
        "bg-yellow-50 text-yellow-800 ring-yellow-600/20",
        "bg-green-50 text-green-700 ring-green-600/20",
        "bg-blue-50  text-blue-700 ring-blue-700/10",
        "bg-indigo-50 text-indigo-700 ring-indigo-700/10",
        "bg-purple-50 text-purple-700 ring-purple-700/10",
        "bg-pink-50 text-pink-700 ring-pink-700/10"
    };

            var randomIndex = new Random().Next(pastelColors.Count);
            return pastelColors[randomIndex];
        }
        public static string NewPostNotificationEmail(Post post)
        {
            string subject = "New Post Notification";
            string message = $@"<!DOCTYPE html>
<html>
<head>
    <style>
        /* Add your email styles here */
    </style>
</head>
<body>
    <div style=""max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1;"">
        <h1 style=""color: #007BFF;"">New Post Notification 📢</h1>
        <p style=""font-size: 16px; line-height: 1.5;"">
            Hi {post.Creator.Fullname}'s followers,
        </p>
        <p style=""font-size: 16px; line-height: 1.5;"">
            We are excited to inform you that {post.Creator.Fullname} has published a new post titled &quot;{post.Title}&quot; on our platform.
        </p>
        <p style=""font-size: 16px; line-height: 1.5;"">
            You can read the post by following this link: [Insert link to the post].
        </p>
        <p style=""font-size: 16px; line-height: 1.5;"">
            If you have any questions or need further assistance, please don't hesitate to contact us.
        </p>
        <p style=""font-size: 16px; line-height: 1.5;"">
            Enjoy reading!
        </p>
        <p style=""font-size: 16px; line-height: 1.5;"">
            Sincerely,
        </p>
    </div>
</body>
</html>";

            return message;
        }
        public static string PostApprovalEmail(bool isSucceed, Post post)
        {
            string subject = isSucceed ? "Post Approval Notification" : "Post Rejection Notification";
            string result = isSucceed ? "approved" : "rejected";
            string message = $@"<!DOCTYPE html>
                    <html>
                    <head>
                        <style>
                            /* Add your email styles here */
                        </style>
                    </head>
                    <body>
                        <div style=""max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1;"">
                            <h1 style=""color: #007BFF;"">Post {result} Notification 🎉</h1>
                            <p style=""font-size: 16px; line-height: 1.5;"">
                                Hi {post.Creator.Fullname},
                            </p>
                            <p style=""font-size: 16px; line-height: 1.5;"">
                                We want to inform you that your post &quot;{post.Title}&quot; has been {result} by our team.
                                </ p >


                                    < p style = ""font - size: 16px; line - height: 1.5; "" >
                                Thank you for using our system. If you have any questions or need further assistance, please don't hesitate to contact us.
                                </ p >


                                    < p style = ""font - size: 16px; line - height: 1.5; "" >
                                        Sincerely,
                            </ p >
                        </ div >
                    </ body >
                    </ html > ";

            return message;
        }
        public static string SystemApprovalEmail(bool isSucceed)
        {
            string succeed = @"
                <body style=""font-family: Arial, sans-serif; background-color: #f2f2f2; margin: 0; padding: 0;"">
                  <div style=""max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1;"">
                    <h1 style=""color: #007BFF;"">Congratulations on Your Approval! 🎉</h1>
                    <p style=""font-size: 16px; line-height: 1.5;"">
                      Hi,
                    </p>
                    <p style=""font-size: 16px; line-height: 1.5;"">
                      We are delighted to inform you that your registration with our system has been approved by our dedicated administrative team. Congratulations on becoming an official member of our community!
                    </p>
                    <p style=""font-size: 16px; line-height: 1.5;"">
                      This is an exciting milestone, and we are thrilled to have you on board. Your approval signifies that you are now part of a vibrant and dynamic community dedicated to [briefly describe your system's purpose and mission].
                    </p>
                    <p style=""font-size: 16px; line-height: 1.5;"">
                      Now that you're an approved member, we encourage you to explore all the fantastic features and benefits of our system:
                    </p>
                    <ul style=""font-size: 16px; line-height: 1.5; margin-left: 20px;"">
                    </ul>
                    <p style=""font-size: 16px; line-height: 1.5;"">
                      We are confident that you will find our system to be a valuable resource that will [mention how your system will benefit the user]. Please take some time to familiarize yourself with the platform and its offerings.
                    </p>
                    <p style=""font-size: 16px; line-height: 1.5;"">
                      If you have any questions or require assistance, our support team is ready to help. Feel free to reach out to us at [support email] for any inquiries or concerns you may have.
                    </p>
                    <p style=""font-size: 16px; line-height: 1.5;"">
                      Once again, congratulations on your approval! We look forward to seeing you thrive and make the most of your experience with us. Your presence adds to the richness of our community, and we can't wait to see the positive contributions you will make.
                    </p>
                    <p style=""font-size: 16px; line-height: 1.5;"">
                      Thank you for choosing our system, and we're excited to have you as part of our community.
                    </p>
                    <p style=""font-size: 16px; line-height: 1.5;"">Warm regards,</p>
                  </div>
                </body>
            ";
            string reject = @"
                <body style=""font-family: Arial, sans-serif; background-color: #f2f2f2; margin: 0; padding: 0;"">
                  <div style=""max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1;"">
                    <h1 style=""color: #FF3333;"">We Regret to Inform You</h1>
                    <p style=""font-size: 16px; line-height: 1.5;"">
                      Hi,
                    </p>
                    <p style=""font-size: 16px; line-height: 1.5;"">
                      We regret to inform you that your registration with our system has not been approved by our administrative team. We appreciate your interest but, unfortunately, we cannot proceed with your application at this time.
                    </p>
                    <p style=""font-size: 16px; line-height: 1.5;"">
                      While we value every application we receive, we have carefully reviewed your submission and found that it does not meet our current requirements or criteria.
                    </p>
                    <p style=""font-size: 16px; line-height: 1.5;"">
                      If you have any further questions or would like additional information regarding this decision, please feel free to reach out to us at.
                    </p>
                    <p style=""font-size: 16px; line-height: 1.5;"">
                      We appreciate your interest and wish you the best in your endeavors. Thank you for considering our system.
                    </p>
                    <p style=""font-size: 16px; line-height: 1.5;"">Sincerely</p>
                  </div>
                </body>
            ";
            return isSucceed ? succeed : reject;
        }
        public static int CountWords(string content)
        {
            // Use a simple approach to count words (not perfect, but works for demonstration)
            if (string.IsNullOrWhiteSpace(content))
                return 0;

            string[] words = content.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }
    }
}
