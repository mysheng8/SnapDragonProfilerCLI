using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200007A RID: 122
	public class CommandManager : ICommandManager
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002AB RID: 683 RVA: 0x00008B2C File Offset: 0x00006D2C
		public bool CanUndo
		{
			get
			{
				return this.m_undo_stack.Count > 0;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002AC RID: 684 RVA: 0x00008B4C File Offset: 0x00006D4C
		public bool CanRedo
		{
			get
			{
				return this.m_redo_stack.Count > 0;
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00008B87 File Offset: 0x00006D87
		public void ExecuteCommand(ICommand command)
		{
			command.Execute();
			if (command.ClearsUndo)
			{
				this.m_undo_stack.Clear();
			}
			else
			{
				this.m_undo_stack.Push(command);
			}
			this.m_redo_stack.Clear();
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00008BBC File Offset: 0x00006DBC
		public void Undo()
		{
			if (this.CanUndo)
			{
				ICommand command = this.m_undo_stack.Pop();
				command.Undo();
				this.m_redo_stack.Push(command);
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00008BF0 File Offset: 0x00006DF0
		public void Redo()
		{
			if (this.CanRedo)
			{
				ICommand command = this.m_redo_stack.Pop();
				command.Redo();
				this.m_undo_stack.Push(command);
			}
		}

		// Token: 0x040001B5 RID: 437
		private Stack<ICommand> m_undo_stack = new Stack<ICommand>();

		// Token: 0x040001B6 RID: 438
		private Stack<ICommand> m_redo_stack = new Stack<ICommand>();
	}
}
