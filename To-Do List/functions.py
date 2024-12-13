import tkinter as tk
from tkinter import messagebox

# Initialize the tasks list
tasks = []

# Functions for task operations
def add_task():
    task = task_entry.get().strip()
    if task:
        tasks.append({"task": task, "completed": False})
        update_task_list()
        task_entry.delete(0, tk.END)
        messagebox.showinfo("Success", "Task added successfully!")
    else:
        messagebox.showwarning("Warning", "Task cannot be empty!")

def update_task_list():
    task_listbox.delete(0, tk.END)
    for i, task in enumerate(tasks):
        status = "✓" if task['completed'] else "x"
        task_listbox.insert(tk.END, f"{i+1}. {task['task']} [{status}]")

def mark_task_completed():
    selected = task_listbox.curselection()
    if selected:
        index = selected[0]
        tasks[index]['completed'] = True
        update_task_list()
        messagebox.showinfo("Success", "Task marked as completed!")
    else:
        messagebox.showwarning("Warning", "No task selected!")

def delete_task():
    selected = task_listbox.curselection()
    if selected:
        index = selected[0]
        removed_task = tasks.pop(index)
        update_task_list()
        messagebox.showinfo("Success", f"Task '{removed_task['task']}' deleted!")
    else:
        messagebox.showwarning("Warning", "No task selected!")

# GUI Setup
root = tk.Tk()
root.title("To-Do List")
root.geometry("400x400")

# Widgets
task_entry = tk.Entry(root, width=40)
task_entry.pack(pady=10)

add_button = tk.Button(root, text="Add Task", command=add_task)
add_button.pack()

task_listbox = tk.Listbox(root, width=50, height=15)
task_listbox.pack(pady=10)

mark_completed_button = tk.Button(root, text="Mark as Completed", command=mark_task_completed)
mark_completed_button.pack()

delete_button = tk.Button(root, text="Delete Task", command=delete_task)
delete_button.pack()

exit_button = tk.Button(root, text="Exit", command=root.quit)
exit_button.pack(pady=10)

# Start the GUI loop
root.mainloop()
