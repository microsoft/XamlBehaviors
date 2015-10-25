// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Windows.UI.Xaml;

namespace Microsoft.Xaml.Interactivity
{
  /// <summary>
  /// A base class for behaviors making them code compatible with older frameworks,
  /// and allow for typed associtated objects.
  /// </summary>
  /// <typeparam name="T">The object type to attach to</typeparam>
  public abstract class Behavior<T> : Behavior where T: DependencyObject
  {
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new T AssociatedObject { get; set; }

    public override void Attach(DependencyObject associatedObject)
    {
      base.Attach(associatedObject);
      this.AssociatedObject = (T)associatedObject;
      OnAttached();
    }

    public override void Detach()
    {
      base.Detach();
      OnDetaching();
    }

    protected virtual void OnAttached()
    {
    }

    protected virtual void OnDetaching()
    {
    }

  }
}
