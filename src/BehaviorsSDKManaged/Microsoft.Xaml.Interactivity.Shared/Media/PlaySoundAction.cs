// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Windows.Media.Playback;
using Windows.Media.Core;

#if WinUI
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MediaPlayerElement = Microsoft.UI.Xaml.Controls.MediaPlayerElement;
using Microsoft.UI.Xaml.Controls.Primitives;
#else
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MediaPlayerElement = Windows.UI.Xaml.Controls.MediaPlayerElement;
using Windows.UI.Xaml.Controls.Primitives;
#endif

namespace Microsoft.Xaml.Interactivity
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
        private readonly DispatcherQueue _queue = DispatcherQueue.GetForCurrentThread();

		private const string MsAppXSchemeFormatString = "ms-appx:///{0}";

		/// <summary>
		/// Identifies the <seealso cref="Source"/> dependency property.
		/// </summary>
		[SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
			nameof(Source),
			typeof(string),
			typeof(PlaySoundAction),
			new PropertyMetadata(null));

		/// <summary>
		/// Identifies the <seealso cref="Volume"/> dependency property.
		/// </summary>
		[SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register(
			nameof(Volume),
			typeof(double),
			typeof(PlaySoundAction),
			new PropertyMetadata(0.5));

		private Popup _popup;

        /// <summary>
        /// Gets or sets the location of the sound file. This is used to set the source property of a <see cref="MediaPlayerElement"/>. This is a dependency property.
        /// </summary>
        /// <remarks>
        /// The sound can be any file format supported by <see cref="MediaPlayerElement"/>. In the case of a video, it will play only the
        /// audio portion.
        /// </remarks>
        public string Source
		{
			get
			{
				return (string)this.GetValue(SourceProperty);
			}
			set
			{
				this.SetValue(SourceProperty, value);
			}
		}

        /// <summary>
        /// Gets or set the volume of the sound. This is used to set the <see cref="MediaPlayer.Volume"/> property of the <see cref="MediaPlayerElement"/>. This is a dependency property.
        /// </summary>
        /// <remarks>
        /// By default this is set to 0.5.
        /// </remarks>
        public double Volume
		{
			get
			{
				return (double)this.GetValue(VolumeProperty);
			}
			set
			{
				this.SetValue(VolumeProperty, value);
			}
		}

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>True if <see cref="MediaPlayerElement.Source"/> is set successfully; else false.</returns>
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
				string absoluteSource = string.Format(CultureInfo.InvariantCulture, MsAppXSchemeFormatString, this.Source);
				if (!Uri.TryCreate(absoluteSource, UriKind.Absolute, out sourceUri))
				{
					return false;
				}
			}

			_popup = new Popup();
            if (sender is UIElement element && element.XamlRoot != null)
            {
                _popup.XamlRoot = element.XamlRoot;
            }

            MediaPlayerElement mediaElement = new MediaPlayerElement();
			_popup.Child = mediaElement;

			// It is legal (although not advisable) to provide a video file. By setting visibility to collapsed, only the sound track should play.
			mediaElement.Visibility = Visibility.Collapsed;

            mediaElement.Source = MediaSource.CreateFromUri(sourceUri);
            mediaElement.AutoPlay = true;
            mediaElement.MediaPlayer.Volume = this.Volume;
            mediaElement.MediaPlayer.MediaEnded += this.MediaElement_MediaEnded;
            mediaElement.MediaPlayer.MediaFailed += this.MediaPlayer_MediaFailed;

            this._popup.IsOpen = true;
			return true;
		}

        private void MediaPlayer_MediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
		{
            // TODO: We should probably have some system/properties to report/bubble errors here
            ClosePopup();
		}

        private void MediaElement_MediaEnded(MediaPlayer sender, object args)
        {
            ClosePopup();
		}

        private void ClosePopup()
        {
            void closePopupImpl()
            {
                if (this._popup != null)
                {
                    this._popup.IsOpen = false;
                    this._popup.Child = null;
                    this._popup = null;
                }
            }

            if (_queue.HasThreadAccess)
            {
                closePopupImpl();
            }
            else
            {
                // In WinUI3 the Media events are called on a background thread, so ensure we're on the UI thread to modify our popup container.
                _queue.TryEnqueue(DispatcherQueuePriority.Normal, closePopupImpl);
            }            
        }        
	}
}
