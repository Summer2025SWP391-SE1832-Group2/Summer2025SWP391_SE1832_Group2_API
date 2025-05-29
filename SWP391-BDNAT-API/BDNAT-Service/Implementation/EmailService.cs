using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message, string username);
    }
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("dnatestingsystem@gmail.com", "kset ewdq konw mzbh"),
                EnableSsl = true,
            };
        }

        public async Task SendEmailAsync(string email, string subject, string code, string usename)
        {
            try
            {
                var bodyHtml = GenerateVerificationEmailHtml(code, usename);

                var mailMessage = new MailMessage("dnatestingsystem@gmail.com", email, subject, bodyHtml);
                mailMessage.IsBodyHtml = true;

                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                throw new Exception($"SMTP error: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }


        public string GenerateVerificationEmailHtml(string code, string username)
        {
            return $@"
    <!DOCTYPE html>
    <html>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <style>
            body {{
                font-family: 'Segoe UI', 'Arial', sans-serif;
                background: linear-gradient(135deg, #E3F2FD 0%, #BBDEFB 100%);
                margin: 0;
                padding: 20px;
                color: #263238;
                line-height: 1.6;
            }}
            .container {{
                max-width: 600px;
                margin: auto;
                background: #FFFFFF;
                border-radius: 16px;
                box-shadow: 0 8px 32px rgba(13, 71, 161, 0.12);
                overflow: hidden;
                border: 1px solid #E1F5FE;
            }}
            .header {{
                background: linear-gradient(135deg, #1976D2 0%, #2196F3 100%);
                color: white;
                text-align: center;
                padding: 40px 30px;
                position: relative;
            }}
            .header::before {{
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                background: url('data:image/svg+xml,<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 100 100""><defs><pattern id=""dna"" x=""0"" y=""0"" width=""20"" height=""20"" patternUnits=""userSpaceOnUse""><circle cx=""10"" cy=""5"" r=""1"" fill=""rgba(255,255,255,0.1)""/><circle cx=""5"" cy=""15"" r=""1"" fill=""rgba(255,255,255,0.1)""/></pattern></defs><rect width=""100"" height=""100"" fill=""url(%23dna)""/></svg>') repeat;
                opacity: 0.3;
            }}
            .header-content {{
                position: relative;
                z-index: 1;
            }}
            .logo {{
                font-size: 36px;
                margin-bottom: 10px;
            }}
            .header h1 {{
                margin: 0;
                font-size: 32px;
                font-weight: 700;
                letter-spacing: -0.5px;
            }}
            .header p {{
                margin: 8px 0 0 0;
                font-size: 16px;
                opacity: 0.9;
                font-weight: 300;
            }}
            .content {{
                padding: 40px 30px;
            }}
            .greeting {{
                font-size: 18px;
                color: #1565C0;
                margin-bottom: 25px;
                font-weight: 500;
            }}
            .message {{
                font-size: 16px;
                color: #37474F;
                margin-bottom: 30px;
            }}
            .code-section {{
                text-align: center;
                margin: 35px 0;
            }}
            .code-label {{
                font-size: 14px;
                color: #546E7A;
                text-transform: uppercase;
                letter-spacing: 1px;
                margin-bottom: 15px;
                font-weight: 600;
            }}
            .code-box {{
                display: inline-block;
                font-size: 32px;
                font-weight: 700;
                padding: 20px 30px;
                background: linear-gradient(135deg, #E3F2FD 0%, #F3E5F5 100%);
                border: 2px solid #2196F3;
                border-radius: 12px;
                letter-spacing: 6px;
                color: #1565C0;
                box-shadow: 0 4px 20px rgba(33, 150, 243, 0.15);
                font-family: 'Courier New', monospace;
            }}
            .info-box {{
                background: #F8F9FA;
                border-left: 4px solid #2196F3;
                padding: 20px;
                border-radius: 0 8px 8px 0;
                margin: 25px 0;
            }}
            .info-box .icon {{
                font-size: 20px;
                color: #1976D2;
                margin-right: 8px;
            }}
            .warning-box {{
                background: #FFF3E0;
                border: 1px solid #FFB74D;
                border-radius: 8px;
                padding: 18px;
                margin: 25px 0;
                color: #E65100;
            }}
            .warning-box .icon {{
                font-size: 18px;
                margin-right: 8px;
            }}
            .features {{
                display: flex;
                justify-content: space-around;
                margin: 30px 0;
                text-align: center;
            }}
            .feature {{
                flex: 1;
                padding: 0 10px;
            }}
            .feature-icon {{
                font-size: 24px;
                color: #2196F3;
                margin-bottom: 8px;
            }}
            .feature-text {{
                font-size: 13px;
                color: #546E7A;
                font-weight: 500;
            }}
            .footer {{
                background: #FAFAFA;
                padding: 30px;
                text-align: center;
                border-top: 1px solid #E0E0E0;
            }}
            .footer-logo {{
                font-size: 20px;
                color: #1976D2;
                margin-bottom: 15px;
                font-weight: 600;
            }}
            .footer p {{
                margin: 8px 0;
                font-size: 14px;
                color: #757575;
            }}
            .contact-info {{
                margin-top: 20px;
                padding-top: 20px;
                border-top: 1px dashed #BDBDBD;
            }}
            @media (max-width: 600px) {{
                .container {{
                    margin: 10px;
                    border-radius: 12px;
                }}
                .header {{
                    padding: 30px 20px;
                }}
                .content {{
                    padding: 30px 20px;
                }}
                .code-box {{
                    font-size: 28px;
                    padding: 18px 25px;
                    letter-spacing: 4px;
                }}
                .features {{
                    flex-direction: column;
                    gap: 20px;
                }}
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <div class='header-content'>
                    <div class='logo'>🧬</div>
                    <h1>DNA Testing System</h1>
                    <p>Advanced Genetic Analysis & Healthcare Solutions</p>
                </div>
            </div>

            <div class='content'>
                <div class='greeting'>
                    Xin chào {username},
                </div>

                <div class='message'>
                    <p>Cảm ơn bạn đã đăng ký sử dụng <strong>DNA Testing System</strong> - hệ thống xét nghiệm di truyền hàng đầu với công nghệ tiên tiến và độ chính xác cao.</p>
                    <p>Để hoàn tất quá trình đăng ký tài khoản, vui lòng sử dụng mã xác thực bên dưới:</p>
                </div>

                <div class='code-section'>
                    <div class='code-label'>Mã Xác Thực</div>
                    <div class='code-box'>{code}</div>
                </div>

                <div class='info-box'>
                    <p><span class='icon'>⏰</span><strong>Thời hạn:</strong> Mã xác thực này có hiệu lực trong <strong>10 phút</strong> kể từ khi nhận được email.</p>
                </div>

                <div class='warning-box'>
                    <p><span class='icon'>🔒</span><strong>Bảo mật:</strong> Nếu bạn không yêu cầu mã này, vui lòng bỏ qua email hoặc liên hệ với chúng tôi ngay lập tức.</p>
                </div>

                <div class='features'>
                    <div class='feature'>
                        <div class='feature-icon'>🔬</div>
                        <div class='feature-text'>Công nghệ<br>Tiên tiến</div>
                    </div>
                    <div class='feature'>
                        <div class='feature-icon'>🎯</div>
                        <div class='feature-text'>Độ chính xác<br>Cao</div>
                    </div>
                    <div class='feature'>
                        <div class='feature-icon'>🏥</div>
                        <div class='feature-text'>Đội ngũ<br>Chuyên nghiệp</div>
                    </div>
                    <div class='feature'>
                        <div class='feature-icon'>🔐</div>
                        <div class='feature-text'>Bảo mật<br>Tuyệt đối</div>
                    </div>
                </div>

                <div class='message'>
                    <p>Chúng tôi cam kết mang đến cho bạ

                    những dịch vụ xét nghiệm di truyền chất lượng cao nhất, hỗ trợ chăm sóc sức khỏe và ra quyết định y tế quan trọng.</p>
                </div>
            </div>

            <div class='footer'>
                <div class='footer-logo'>🧬 DNA Testing System</div>
                <p>&copy; {DateTime.Now.Year} DNA Testing System. Bảo lưu mọi quyền.</p>
                <p>Hệ thống xét nghiệm di truyền hàng đầu Việt Nam</p>
                
                <div class='contact-info'>
                    <p><strong>Hỗ trợ khách hàng:</strong> support@dnatesting.com</p>
                    <p><strong>Hotline:</strong> 1900-DNA-TEST (1900-362-8378)</p>
                    <p><strong>Website:</strong> www.dnatestingsystem.com</p>
                </div>
            </div>
        </div>
    </body>
    </html>
    ";
        }

    }
}
