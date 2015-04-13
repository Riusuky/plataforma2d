using System.Collections;
using UnityEngine;
public class ConversationManager : Singleton<ConversationManager > {
    protected ConversationManager() { } // guarantee this will be always a singleton only - can't use the constructor!

    //Is there a converastion going on
    bool talking = false;

    //The current line of text being displayed
    ConversationEntry currentConversationLine;

    //Estimated width of characters in the font
    int fontSpacing = 7;

    //How wide does the dialog window need to be
    int conversationTextWidth;

    //How high does the dialog window need to be
    int dialogHeight = 70;

    //Offset space needed for character image
    public int displayTextureOffset = 70;

    //Scaled image rectangle for displaying character image
    Rect scaledTextureRect;

    public void StartConversation(Conversation conversation)
    {
        //Start displying the supplied conversation
        if (!talking)
        {
            StartCoroutine(DisplayConversation(conversation));
        }
    }

    IEnumerator DisplayConversation(Conversation conversation)
    {
        talking = true;
        foreach (var conversationLine in conversation.ConversationLines)
        {
            currentConversationLine = conversationLine;
            conversationTextWidth = currentConversationLine.ConversationText.Length * fontSpacing;
            scaledTextureRect = new Rect(
                currentConversationLine.DisplayPic.textureRect.x / currentConversationLine.DisplayPic.texture.width,
                currentConversationLine.DisplayPic.textureRect.y / currentConversationLine.DisplayPic.texture.height,
                currentConversationLine.DisplayPic.textureRect.width / currentConversationLine.DisplayPic.texture.width,
                currentConversationLine.DisplayPic.textureRect.height / currentConversationLine.DisplayPic.texture.height);
            yield return new WaitForSeconds(3);
        }
        talking = false;
    }

    void OnGUI()
    {
        if (talking)
        {
            //layout start
            GUI.BeginGroup(new Rect(Screen.width / 2 - conversationTextWidth / 2, 50, conversationTextWidth + displayTextureOffset + 10, dialogHeight));

            //the background box
            GUI.Box(new Rect(0, 0, conversationTextWidth + displayTextureOffset + 10, dialogHeight), "");

            //the character name
            GUI.Label(new Rect(displayTextureOffset, 10, conversationTextWidth + 30, 20), currentConversationLine.SpeakingCharacterName);

            //the conversation text
            GUI.Label(new Rect(displayTextureOffset, 30, conversationTextWidth + 30, 20), currentConversationLine.ConversationText);

            //the character image
            GUI.DrawTextureWithTexCoords(new Rect(10, 10, 50, 50), currentConversationLine.DisplayPic.texture, scaledTextureRect);

            //layout end
            GUI.EndGroup(); 
        }
    }
}

