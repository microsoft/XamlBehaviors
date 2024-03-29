﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// TODO: WinUI3 preview4 removed MediaElement
#if !WinUI
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Xaml.Interactions.Media
{
	/// <summary>
	/// An action that will play a sound to completion.
	/// </summary>
	/// <remarks>
	/// This action is intended for use with short sound effects that don't need to be stopped or controlled. If you are trying 
	/// to create a music player or game, it may not meet your needs.
	/// </remarks>
	public sealed class PlaySoundAction : DependencyObject, IAction
	{
		private const string MsAppXSchemeFormatString = "ms-appx:///{0}";

		/// <summary>
		/// Identifies the <seealso cref="Source"/> dependency property.
		/// </summary>
		[SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
			"Source",
			typeof(string),
			typeof(PlaySoundAction),
			new PropertyMetadata(null));

		/// <summary>
		/// Identifies the <seealso cref="Volume"/> dependency property.
		/// </summary>
		[SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register(
			"Volume",
			typeof(double),
			typeof(PlaySoundAction),
			new PropertyMetadata(0.5));

		private Popup _popup;

        /// <summary>
        /// Gets or sets the location of the sound file. This is used to set the source property of a <see cref="MediaElement"/>. This is a dependency property.
        /// </summary>
        /// <remarks>
        /// The sound can be any file format supported by <see cref="MediaElement"/>. In the case of a video, it will play only the
        /// audio portion.
        /// </remarks>
        public string Source
		{
			get
			{
				return (string)this.GetValue(PlaySoundAction.SourceProperty);
			}
			set
			{
				this.SetValue(PlaySoundAction.SourceProperty, value);
			}
		}

        /// <summary>
        /// Gets or set the volume of the sound. This is used to set the <see cref="MediaElement.Volume"/> property of the <see cref="MediaElement"/>. This is a dependency property.
        /// </summary>
        /// <remarks>
        /// By default this is set to 0.5.
        /// </remarks>
        public double Volume
		{
			get
			{
				return (double)this.GetValue(PlaySoundAction.VolumeProperty);
			}
			set
			{
				this.SetValue(PlaySoundAction.VolumeProperty, value);
			}
		}

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>True if <see cref="MediaElement.Source"/> is set successfully; else false.</returns>
        public object Execute(object sender, object parameter)
		{
			if (string.IsNullOrEmpty(this.Source))
			{
				return false;
			}

			Uri sourceUri;
			if (!Uri.TryCreate(this.Source, UriKind.Absolute, out sourceUri))
			{
				// Impose ms-appx:// scheme if user has specified a relative URI
				string absoluteSource = string.Format(CultureInfo.InvariantCulture, PlaySoundAction.MsAppXSchemeFormatString, this.Source);
				if (!Uri.TryCreate(absoluteSource, UriKind.Absolute, out sourceUri))
				{
					return false;
				}
			}

			this._popup = new Popup();
			MediaElement mediaElement = new MediaElement();
			_popup.Child = mediaElement;

			// It is legal (although not advisable) to provide a video file. By setting visibility to collapsed, only the sound track should play.
			mediaElement.Visibility = Visibility.Collapsed;
			mediaElement.Source = sourceUri;
			mediaElement.Volume = this.Volume;

			mediaElement.MediaEnded += this.MediaElement_MediaEnded;
			mediaElement.MediaFailed += this.MediaElement_MediaFailed;

            this._popup.IsOpen = true;
			return true;
		}

		private void MediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
		{
			if (this._popup != null)
			{
				this._popup.IsOpen = false;
				this._popup.Child = null;
				this._popup = null;
			}
		}

		private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
		{
			if (this._popup != null)
			{
				this._popup.IsOpen = false;
				this._popup.Child = null;
				this._popup = null;
			}
		}
	}
}
#endif