using ExpoShared.Domain.Entities.Chats.Shared;
using ExpoShared.Domain.Entities.Users;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace ExpoApp.Api.Hubs;

public class BaseGoogleNotificationHub(IUserRepository userRepository) : Hub
{
	protected async Task SendPushNotification(ReceiveMessageDto msgDto)
	{
	    var receiver = await userRepository.GetByIdAsync(msgDto.ReceiverId);
	    var sender = await userRepository.GetByIdAsync(msgDto.SenderId);

	    var message = new Message()
	    {
	        Notification = new Notification()
	        {
	            Title = msgDto.SenderName,
	        },
	        Data = new Dictionary<string, string>()
	        {
	            { "roomId", msgDto.RoomId },
	            { "senderId", msgDto.SenderId.ToString() },
	            { "message", msgDto.TranslatedMessage ?? msgDto.SendedMessage },
	            { "fileUri", msgDto.File ?? "" },
	            { "profileImage", sender.ProfileImageUri ?? "" },
	            { "receiverId", msgDto.ReceiverId.ToString()},
	        },
	        Token = receiver.FcmToken
	    };

	    // Verifica se há um arquivo e determina o tipo
	    if (!string.IsNullOrEmpty(msgDto.File) && !string.IsNullOrEmpty(msgDto.FileName))
	    {
	        var extension = Path.GetExtension(msgDto.FileName)?.ToLowerInvariant()?.TrimStart('.');
	        var imageExtensions = new[] { "jpg", "jpeg", "png", "gif", "bmp", "webp" };
	        var videoExtensions = new[] { "mp4", "mov", "avi", "mkv", "webm" };

	        if (imageExtensions.Contains(extension))
	        {
	            // Para imagens, define ImageUrl para exibir a imagem
	            message.Notification.ImageUrl = msgDto.File;
	        }
	        else if (videoExtensions.Contains(extension))
	        {
	            // Para vídeos, define o corpo como "Video received"
	            message.Notification.Body = "Video received";
	        }
	        else
	        {
	            // Para outros arquivos, define o corpo como "File received"
	            message.Notification.Body = "File received";
	        }
	    }
	    else
	    {
	        // Sem arquivo, usa a mensagem padrão
	        message.Notification.Body = msgDto.TranslatedMessage ?? msgDto.SendedMessage;
	    }

	    try
	    {
	        string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
	        Console.WriteLine($"Successfully sent push notification: {response}");
	    }
	    catch (Exception ex)
	    {
	        Console.WriteLine($"Error sending push notification: {ex.Message}");
	    }
	}
}
