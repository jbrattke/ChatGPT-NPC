using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenAI_API;
using OpenAI_API.Chat;

public class NPCBehavior : MonoBehaviour
{
    [TextArea(2, 10)]
    public string npcIntro;
    [TextArea(3, 10)]
    public string npcDescription;
    public string[] examplePrompts;
    public string[] exampleResponses;

    public Text npcResponseText;
    public InputField userInputField;

    static OpenAIAPI api;
    Conversation conversation;
    bool isConversationActive;

    // Start is called before the first frame update
    void Start()
    {
        api = new OpenAIAPI("YOUR_API_KEY_HERE");
    }

    void Update()
    {
        if (isConversationActive && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleUI();
        }
    }

    public void StartConversation()
    {
        var chat = api.Chat.CreateConversation();
        chat.AppendSystemMessage(npcDescription);

        for (int i = 0; i < examplePrompts.Length; i++)
        {
            chat.AppendUserInput(examplePrompts[i]);
            chat.AppendExampleChatbotOutput(exampleResponses[i]);
        }

        conversation = chat;

        ToggleUI();
    }

    async void SendChatMessage(string message) {
        conversation.AppendUserInput(message);
        npcResponseText.text = "...";

        string response = await conversation.GetResponseFromChatbot();
        npcResponseText.text = response;
    }

    void ToggleUI() {
        MouseLook.ToggleUI();

        userInputField.gameObject.SetActive(MouseLook.isUIActive);
        npcResponseText.gameObject.SetActive(MouseLook.isUIActive);

        if (MouseLook.isUIActive)
        {
            userInputField.onSubmit.AddListener(delegate { SendChatMessage(userInputField.text); });
            npcResponseText.text = npcIntro;
            userInputField.text = "";
            isConversationActive = true;
        }
        else
        {
            userInputField.onSubmit.RemoveAllListeners();
            isConversationActive = false;
        }
    }
}
