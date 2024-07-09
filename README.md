
# Sample Project With MailKit

In this project, I leverage MailKit to seamlessly manage email communication with a test MailService. The selected Mail service is GreenMail, a reliable tool designed for testing email functionality.

For detailed information on GreenMail, visit their website: [GreenMail Official Page](https://greenmail-mail-test.github.io/greenmail/)

## Key Components

**MailKit Integration**: Utilizing MailKit, the project establishes a reliable connection with external mail servers, enabling seamless transmission and retrieval of emails. 
MailKit's comprehensive features facilitate smooth communication protocols, including SMTP, IMAP, and POP3, ensuring compatibility with various mail server configurations.


**GreenMail for Testing**: GreenMail serves as the primary testing environment within the project, providing a lightweight, in-memory mail server solution. Its versatile capabilities allow for the simulation of various email scenarios, including sending, receiving, and manipulation of messages, all within a controlled testing environment. GreenMail's flexibility enables comprehensive testing of email functionalities, ensuring the robustness and reliability of the system.

## GreenMail Setup

To set up GreenMail using a Docker Compose file named "docker-compose.yml" you can run it using this powershell "deploy-docker-compose.ps1".

Docker Compose setting allows GreenMail will be accessible via the following ports:
 - SMTP: 3143
 - HTTP (for the GreenMail web interface): 8080
 - POP3: 3025

You can now interact with GreenMail for testing purposes. 

## Basic Flow

Utilizing Swagger, users can effortlessly dispatch default emails, initiating the communication process. Subsequently, a meticulously crafted timer job, engineered with Quartz.NET, efficiently parses incoming emails and echoes their contents onto the console for convenient monitoring and debugging.

## Send Mail Flow

The MailSenderService class within the Mail.Hub.Domain project encapsulates functionality for sending emails using the MailKit library. This service provides a simple and efficient way to send emails from within a .NET application.


## Receive Mail Flow

This ReceiverMailService class encapsulates the functionality to connect to the email server using the IMAP protocol, retrieve new emails, parse them, and handle them accordingly. It utilizes the MailKit library for IMAP operations.

The ParseNewMails method fetches new, unseen emails from the inbox, extracts relevant information such as subject and body, and sends it for further processing using the Mediator pattern.

The IncomeMailsJob class represents a Quartz.NET job responsible for triggering the email parsing process at specified intervals. In the Execute method, it simply invokes the ParseNewMails method of the ReceiverMailService. Any exceptions during execution are logged for debugging purposes.

## Useful Links and Nuget Packages Utilized

To empower this project with advanced scheduling capabilities, Quartz.NET proves instrumental. 

Explore Quartz.NET's features and documentation here: [Quartz.NET Official Website](https://www.quartz-scheduler.net/)

For streamlining communication and promoting decoupling, MediatR serves as an indispensable tool. 

Learn more about MediatR and its benefits on the official GitHub repository: [MediatR GitHub Repository](https://github.com/jbogard/MediatR)
