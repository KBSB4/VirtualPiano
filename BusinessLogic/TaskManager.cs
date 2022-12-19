using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
	public class TaskManager
	{
		Queue<Task> taskQueue = new Queue<Task>();

		public async Task QueueAndWait(Task task)
		{
			taskQueue.Enqueue(task);

			while(taskQueue.Peek() != task)
			{
				await Task.Delay(20);
			}
		}

		public void CompleteTask()
		{
			taskQueue.Dequeue();
		}
	}
}
