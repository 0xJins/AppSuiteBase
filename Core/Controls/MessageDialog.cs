﻿using CarinaStudio.Threading;
using System;
using System.Threading.Tasks;

namespace CarinaStudio.AppSuite.Controls
{
	/// <summary>
	/// Message dialog.
	/// </summary>
	public class MessageDialog:CommonDialog<MessageDialogResult>
	{
		// Fields.
		MessageDialogButtons buttons = MessageDialogButtons.OK;
		MessageDialogResult? defaultResult;
		bool? doNotAskAgain;
		MessageDialogIcon icon = MessageDialogIcon.Information;
		string? message;


		/// <summary>
		/// Get or set buttons shown in dialog.
		/// </summary>
		public MessageDialogButtons Buttons
		{
			get => this.buttons;
			set
			{
				this.VerifyAccess();
				this.VerifyShowing();
				this.buttons = value;
			}
		}


		/// <summary>
		/// Get or set default result of dialog.
		/// </summary>
		public MessageDialogResult? DefaultResult
		{
			get => this.defaultResult;
			set
			{
				this.VerifyAccess();
				this.VerifyShowing();
				this.defaultResult = value;
			}
		}


		/// <summary>
		/// Get or set whether "Do not ask me again" has been checked or not. Set Null to hide the UI.
		/// </summary>
		public bool? DoNotAskAgain
		{
			get => this.doNotAskAgain;
			set
			{
				this.VerifyAccess();
				this.VerifyShowing();
				this.doNotAskAgain = value;
			}
		}


		/// <summary>
		/// Get or set icon shown in dialog.
		/// </summary>
		public MessageDialogIcon Icon
		{
			get => this.icon;
			set
			{
				this.VerifyAccess();
				this.VerifyShowing();
				this.icon = value;
			}
		}


		/// <summary>
		/// Get or set message shown in dialog.
		/// </summary>
		public string? Message
		{
			get => this.message;
			set
			{
				this.VerifyAccess();
				this.VerifyShowing();
				this.message = value;
			}
		}


        /// <summary>
        /// Show message dialog.
        /// </summary>
        /// <param name="owner">Owner window.</param>
        /// <returns>Task to get result of dialog.</returns>
        protected override async Task<MessageDialogResult> ShowDialogCore(Avalonia.Controls.Window owner)
        {
			var dialog = new MessageDialogImpl();
			dialog.Buttons = this.buttons;
			dialog.DefaultResult = this.defaultResult;
			dialog.DoNotAskAgain = this.doNotAskAgain;
			dialog.Icon = this.icon;
			dialog.Message = this.message;
			dialog.Title = this.Title ?? Avalonia.Application.Current?.Name;
			var result = await dialog.ShowDialog<MessageDialogResult>(owner);
			this.doNotAskAgain = dialog.DoNotAskAgain;
			return result;
		}
	}


	/// <summary>
	/// Combination of buttons of <see cref="MessageDialog"/>.
	/// </summary>
	public enum MessageDialogButtons
	{
		/// <summary>
		/// OK.
		/// </summary>
		OK,
		/// <summary>
		/// OK and Cancel.
		/// </summary>
		OKCancel,
		/// <summary>
		/// Yes and No.
		/// </summary>
		YesNo,
		/// <summary>
		/// Yes, No and Cancel.
		/// </summary>
		YesNoCancel,
	}


	/// <summary>
	/// Icon of <see cref="MessageDialog"/>.
	/// </summary>
	public enum MessageDialogIcon
	{
		/// <summary>
		/// Information.
		/// </summary>
		Information,
		/// <summary>
		/// Question.
		/// </summary>
		Question,
		/// <summary>
		/// Success.
		/// </summary>
		Success,
		/// <summary>
		/// Warning.
		/// </summary>
		Warning,
		/// <summary>
		/// Error.
		/// </summary>
		Error,
	}


	/// <summary>
	/// Result of <see cref="MessageDialog"/>
	/// </summary>
	public enum MessageDialogResult
	{
		/// <summary>
		/// OK.
		/// </summary>
		OK,
		/// <summary>
		/// Cancel.
		/// </summary>
		Cancel,
		/// <summary>
		/// Yes.
		/// </summary>
		Yes,
		/// <summary>
		/// No.
		/// </summary>
		No,
	}
}
