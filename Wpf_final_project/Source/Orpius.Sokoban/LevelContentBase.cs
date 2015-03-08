using System.ComponentModel;
using System.Threading;

namespace Orpius.Sokoban
{
	/// <summary>
	/// Base implementaion for all content within
	/// the <see cref="Level"/> grid.
	/// </summary>
	public abstract class LevelContentBase : INotifyPropertyChanged
	{
		SynchronizationContext context = SynchronizationContext.Current;

		/// <summary>
		/// Gets or sets the context used to post 
		/// to the main UI thread.
		/// </summary>
		/// <value>The context used to post to the main
		/// UI thread.</value>
		protected SynchronizationContext Context
		{
			get
			{
				if (context == null)
				{
					context = SynchronizationContext.Current;
				}
				return context;
			}
			set
			{
				context = value;
			}
		}

		protected LevelContentBase()
		{
		}

		#region PropertyChangedEventHandler
		event PropertyChangedEventHandler propertyChanged;

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged
		{
			add
			{
				propertyChanged += value;
			}
			remove
			{
				propertyChanged -= value;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:PropertyChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> 
		/// instance containing the event data.</param>
		void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (propertyChanged != null)
			{
				propertyChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:PropertyChanged"/> event.
		/// </summary>
		/// <param name="property">The name of the property that changed.</param>
		protected void OnPropertyChanged(string property)
		{
			/* We use the SynchronizationContext context
			 to ensure that we don't cause an InvalidOperationException
			 if the property change triggers something occuring
			 in the main UI thread. */
			if (context == null)
			{
				OnPropertyChanged(new PropertyChangedEventArgs(property));
			}
			else
			{
				context.Send(delegate
				{
					OnPropertyChanged(new PropertyChangedEventArgs(property));
				}, null);
			}
		}
		#endregion
	}
}
