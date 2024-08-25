import { v4 as uuidv4 } from 'uuid';
import AWS from 'aws-sdk';

const dynamoDB = new AWS.DynamoDB({ region: 'eu-central-1' });

const apigatewaymanagementapi = new AWS.ApiGatewayManagementApi({
    region: 'eu-central-1',
    endpoint: process.env.SERVICE_URL
});

export const handler = async (event) => {
    const connectionId = event.requestContext.connectionId;
    try {
        const command = JSON.parse(event.body);
        const chat = await getChatByIdAsync(command.chatId);
        console.log(chat);

        const senderConnectionMapping = await getCMByConnectionAsync(connectionId);
        console.log(senderConnectionMapping);

        const message = {
            id: uuidv4().toString(),
            chatId: chat.Id,
            senderId: senderConnectionMapping.Sub,
            text: command.text,
            date: new Date().toISOString()
        };
        await saveMessageAsync(message);

        const sendToId = chat.ClientId === senderConnectionMapping.Sub ? chat.FreelancerId : chat.ClientId;

        const connectionMapping = await getCMBySubAsync(sendToId);

		const responseMessage = {
            action: 'newMessage',
            body: { ...message }
        }

        if (!connectionMapping.ConnectionId) {
            return {
                statusCode: 200,
                body: JSON.stringify(responseMessage)
            };
        }

        await sendMessageToConnectionAsync(connectionMapping.ConnectionId, responseMessage);

        console.log('Message sent successfully');

        return {
            statusCode: 200,
            body: JSON.stringify(responseMessage)
        };

    } catch (error) {
        console.log(`Error: ${error}`);

        return {
            statusCode: 500,
            body: JSON.stringify({ message: 'Internal Server Error' })
        };
    }
};

async function sendMessageToConnectionAsync(connectionId, message) {
    const params = {
        ConnectionId: connectionId,
        Data: JSON.stringify(message)
    };

    try {
        await apigatewaymanagementapi.postToConnection(params).promise();
    } catch (error) {
        console.error(`Error sending message to connection ${connectionId}:`, error);
        throw new Error(`Error sending message to connection ${connectionId}: ${error.message}`);
    }
}

async function getChatByIdAsync(id) {
    console.log(id);
    const params = {
        TableName: 'Chats',
        Key: {
            'Id': { S: id }
        }
    };

    try {
        const result = await dynamoDB.getItem(params).promise();
        return AWS.DynamoDB.Converter.unmarshall(result.Item);
    } catch (error) {
        console.error('Error getting chat by id:', error);
        throw error;
    }
}

async function getCMBySubAsync(sub) {
    const params = {
        TableName: 'ChatConnectionMapping',
        Key: {
            'Sub': {
                S: sub
            }
        }
    };

    try {
        const result = await dynamoDB.getItem(params).promise();
        return AWS.DynamoDB.Converter.unmarshall(result.Item);
    } catch (error) {
        console.error('Error getting connection mapping by sub:', error);
        throw error;
    }
}

async function getCMByConnectionAsync(connectionId) {
    const params = {
        TableName: 'ChatConnectionMapping',
        FilterExpression: 'ConnectionId = :connectionId',
        ExpressionAttributeValues: {
            ':connectionId': { S: connectionId }
        }
    };

    try {
        const result = await dynamoDB.scan(params).promise();
        return AWS.DynamoDB.Converter.unmarshall(result.Items[0]);
    } catch (error) {
        console.error('Error getting connection mapping by connectionId:', error);
        throw error;
    }
}

async function saveMessageAsync(message) {
    const params = {
        TableName: 'Messages',
        Item: {
            'Id': { S: message.id },
            'ChatId': { S: message.chatId },
            'SenderId': { S: message.senderId },
            'Text': { S: message.text },
            'Date': { S: message.date },
        }
    };

    try {
        await dynamoDB.putItem(params).promise();
    } catch (error) {
        console.error('Error saving message:', error);
        throw error;
    }
}
