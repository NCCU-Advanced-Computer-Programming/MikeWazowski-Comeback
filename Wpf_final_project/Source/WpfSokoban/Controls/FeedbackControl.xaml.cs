/*	<File>
		<Author Name="Daniel Vaughan" Email="dbvaughan NOSPAM at NOSPAM gmail NOSPAM dot NOSPAM com"/>
		<CreationDate>2007/10/29 18:09</CreationDate>
		<LastSubmissionDate>$Date: $</LastSubmissionDate>
		<Version>$Revision: $</Version>
		<License>
Copyright © 2007, Daniel Vaughan
All rights reserved.
http://www.orpius.com

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

- Redistributions of source code must retain the above copyright
notice, this list of conditions and the following disclaimer.

- Neither Daniel Vaughan, nor the names of its
contributors may be used to endorse or promote products
derived from this software without specific prior written 
permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
POSSIBILITY OF SUCH DAMAGE.
		</License>
	</File>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Orpius.Sokoban.Controls
{
	/// <summary>
	/// Interaction logic for FeedbackControl.xaml
	/// </summary>
	public partial class FeedbackControl : UserControl
	{
		FeedbackMessage message;

		public FeedbackMessage Message
		{
			get
			{
				return message;//button1.Content.ToString();
			}
			set
			{
				if (value == null || string.IsNullOrEmpty(value.Message))
				{
					button1.Content = string.Empty;
					Visibility = Visibility.Hidden;
				}
				else
				{
					button1.Content = value.Message;
					Visibility = Visibility.Visible;
				}
				message = value;
			}
		}

		public FeedbackControl()
		{
			InitializeComponent();
		}

		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{

		}

		#region Click event
		event EventHandler<FeedbackEventArgs> click;

		public event EventHandler<FeedbackEventArgs> Click
		{
			add
			{
				click += value;
			}
			remove
			{
				click -= value;
			}
		}

		protected void OnClick(FeedbackEventArgs e)
		{
			if (click != null)
			{
				click(this, e);
			}
		}
		#endregion

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			OnClick(new FeedbackEventArgs(
				message != null ? message.CommandArgument : string.Empty));
		}
	}

	public class FeedbackMessage
	{
		public string Message
		{
			get;
			set;
		}

		public object CommandArgument
		{
			get;
			set;
		}
	}

	public class FeedbackEventArgs : EventArgs
	{
		public object CommandArgument
		{
			get;
			private set;
		}

		public FeedbackEventArgs(object commandArgument)
		{
			CommandArgument = commandArgument;
		}
	}
}
