﻿namespace MedLinkApp.Selectors;

internal class ChatTemplateSelector : DataTemplateSelector
{
    public DataTemplate IncomingMessageTemplate { get; set; }
    public DataTemplate OutgoingMessageTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (item is Message message)
        {
            if (message.SenderName != null)
                return OutgoingMessageTemplate; //OutgoingMessageTemplate
            else
                return IncomingMessageTemplate;
        }

        throw new NotImplementedException();
    }
}
