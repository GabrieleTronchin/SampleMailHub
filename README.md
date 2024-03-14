# Sample Project With MailKit

In this project, I leverage MailKit to seamlessly manage email communication with a test MailService. The selected Mail service is GreenMail, a reliable tool designed for testing email functionality.

For detailed information on GreenMail, visit their website: [GreenMail Official Page](https://greenmail-mail-test.github.io/greenmail/)


## Basic Flow

Utilizing Swagger, users can effortlessly dispatch default emails, initiating the communication process. Subsequently, a meticulously crafted timer job, engineered with Quartz.NET, efficiently parses incoming emails and echoes their contents onto the console for convenient monitoring and debugging.

## Useful Links and Nuget Packages Utilized

To empower this project with advanced scheduling capabilities, Quartz.NET proves instrumental. 

Explore Quartz.NET's features and documentation here: [Quartz.NET Official Website](https://www.quartz-scheduler.net/)

For streamlining communication and promoting decoupling, MediatR serves as an indispensable tool. 

Learn more about MediatR and its benefits on the official GitHub repository: [MediatR GitHub Repository](https://github.com/jbogard/MediatR)