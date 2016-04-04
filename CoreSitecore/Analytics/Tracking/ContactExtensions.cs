using Sitecore.Analytics.Model.Entities;
using Sitecore.Analytics.Tracking;
using Sitecore.Social.Connector.Facets.Contact.SocialProfile;
using System;

public static class ContactExtensions
{
    #region Sitecore - Social Profiles

    public const string FacebookNetworkKey = "Facebook";
    public const string GoogleNetworkKey = "Google";
    public const string TwitterNetworkKey = "Twitter";

    public static ISocialProfileFacet GetSocialProfile(this Contact contact)
    {
        return contact.GetFacet<ISocialProfileFacet>("SocialProfile"); // Key must match Sitecore's configuration
    }

    /// <summary>
    /// Gets the first or default, so should only be used for registration social additional info page.
    /// </summary>
    /// <param name="contact"></param>
    /// <returns></returns>
    public static INetworkElement GetSocialProfileNetwork(this Contact contact, string providerKey)
    {
        var socialProfile = contact.GetSocialProfile();
        return socialProfile.Networks.GetSafely(providerKey);
    }

    public static INetworkElement GetFacebookNetwork(this Contact contact)
    {
        return contact.GetSocialProfileNetwork(FacebookNetworkKey);
    }

    public static INetworkElement GetGoogleNetwork(this Contact contact)
    {
        return contact.GetSocialProfileNetwork(GoogleNetworkKey);
    }

    public static INetworkElement GetTwitterNetwork(this Contact contact)
    {
        return contact.GetSocialProfileNetwork(TwitterNetworkKey);
    }

    #endregion


    #region Sitecore - Personal Info

    public static IContactPersonalInfo GetPersonalInfo(this Contact contact)
    {
        return contact.GetFacet<IContactPersonalInfo>("Personal"); // Key must match Sitecore's configuration
    }

    public static string GetFirstName(this Contact contact)
    {
        return (contact == null) ? String.Empty : contact.GetPersonalInfo().FirstName;
    }

    public static string GetSurname(this Contact contact)
    {
        return (contact == null) ? String.Empty : contact.GetPersonalInfo().Surname;
    }

    public static string GetFirstNameInitial(this Contact contact)
    {
        return GetInitial(contact.GetFirstName());
    }

    public static string GetLastNameInitial(this Contact contact)
    {
        return GetInitial(contact.GetFirstName());
    }

    public static string GetInitial(string name)
    {
        return string.IsNullOrWhiteSpace(name) ? String.Empty : name[0].ToString().ToUpper();
    }

    public static string GetInitials(this Contact contact)
    {
        return contact == null ? String.Empty : GetInitials(contact.GetFirstName(), contact.GetSurname());
    }

    public static string GetInitials(string firstName, string lastName)
    {
        return GetInitial(firstName) + GetInitial(lastName);
    }

    #endregion


    #region Sitecore - Addresses

    public static IContactAddresses GetAddresses(this Contact contact)
    {
        return contact.GetFacet<IContactAddresses>("Addresses"); // Key must match Sitecore's configuration
    }

    public static IAddress GetAddress(this Contact contact, string addressKey, bool createIfNotExists = false, bool makePreferred = false)
    {
        var addresses = contact.GetAddresses();

        IAddress address = null;
        if (addresses.Entries.Contains(addressKey))
        {
            address = addresses.Entries[addressKey];
        }
        else if (createIfNotExists)
        {
            address = addresses.Entries.Create(addressKey);
        }

        if (makePreferred)
        {
            addresses.Preferred = addressKey;
        }

        return address;
    }

    #endregion


    #region Sitecore - Emails

    public static IContactEmailAddresses GetEmails(this Contact contact)
    {
        return contact.GetFacet<IContactEmailAddresses>("Emails"); // Key must match Sitecore's configuration
    }

    public static IEmailAddress GetEmail(this Contact contact, string emailKey, bool createIfNotExists = false, bool makePreferred = false)
    {
        var emails = contact.GetEmails();

        IEmailAddress email = null;
        if (emails.Entries.Contains(emailKey))
        {
            email = emails.Entries[emailKey];
        }
        else if (createIfNotExists)
        {
            email = emails.Entries.Create(emailKey);
        }

        if (makePreferred)
        {
            emails.Preferred = emailKey;
        }

        return email;
    }

    #endregion
}
