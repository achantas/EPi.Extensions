﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     ContentReference extensions.
    /// </summary>
    public static class ContentReferenceExtensions
    {
        /// <summary>
        ///     Returns enumeration of child contents of PageData type for provided content reference.
        /// </summary>
        /// <param name="contentReference">Content reference for which child contents to get.</param>
        /// <returns>Enumeration of PageData child content.</returns>
        public static IEnumerable<PageData> GetChildren(this ContentReference contentReference)
        {
            return contentReference.GetChildren<PageData>();
        }

        /// <summary>
        ///     Returns enumeration of child contents of concrete type for provided content reference.
        /// </summary>
        /// <typeparam name="T">Type of child content (IContentData).</typeparam>
        /// <param name="contentReference">Content reference for which child contents to get.</param>
        /// <returns>Enumeration of <typeparamref name="T" /> child content.</returns>
        public static IEnumerable<T> GetChildren<T>(this ContentReference contentReference) where T : IContentData
        {
            if (!contentReference.IsNullOrEmpty())
            {
                var repository = ServiceLocator.Current.GetInstance<IContentLoader>();
                return repository.GetChildren<T>(contentReference);
            }

            return Enumerable.Empty<T>();
        }

        /// <summary>
        ///     Returns page of PageData type for provided content reference.
        /// </summary>
        /// <param name="contentReference">Content reference for which to get page.</param>
        /// <returns>Page of PageData type that match content reference.</returns>
        public static PageData GetPage(this ContentReference contentReference)
        {
            return contentReference.GetPage<PageData>();
        }

        /// <summary>
        ///     Returns page of concrete type for provided content reference.
        /// </summary>
        /// <typeparam name="T">Type of page (PageData).</typeparam>
        /// <param name="contentReference">Content reference for which to get page.</param>
        /// <returns>Page of <typeparamref name="T" /> that match content reference.</returns>
        public static T GetPage<T>(this ContentReference contentReference) where T : PageData
        {
            if (contentReference.IsNullOrEmpty()) return null;

            var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
            return loader.Get<PageData>(contentReference) as T;
        }

        /// <summary>
        ///     Returns friendly URL for provided content reference.
        /// </summary>
        /// <param name="contentReference">Content reference for which to create friendly url.</param>
        /// <param name="includeHost">Mark if include host name in the url.</param>
        /// <returns>String representation of URL for provided content reference.</returns>
        public static string GetFriendlyUrl(this ContentReference contentReference, bool includeHost = false)
        {
            if (contentReference.IsNullOrEmpty()) return string.Empty;

            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            var url = urlResolver.GetUrl(contentReference);

            if (!includeHost)
            {
                return url;
            }

            var siteUri = HttpContext.Current != null
                ? HttpContext.Current.Request.Url
                : SiteDefinition.Current.SiteUrl;

            var urlBuilder = new UrlBuilder(url)
            {
                Scheme = siteUri.Scheme,
                Host = siteUri.Host,
                Port = siteUri.Port
            };

            return urlBuilder.ToString();
        }

        /// <summary>
        ///     Indicates whether the specified content reference is null or an EmptyReference.
        /// </summary>
        /// <param name="contentReference">Content reference to test.</param>
        /// <returns>true if content reference is null or EmptyReference else false</returns>
        public static bool IsNullOrEmpty(this ContentReference contentReference)
        {
            return ContentReference.IsNullOrEmpty(contentReference);
        }
    }
}