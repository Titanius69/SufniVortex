# How to Create Extensions for SufniBrowser

## 📌 Overview
Extensions in **SufniBrowser** allow developers to add new features by dynamically loading external DLL files. These extensions are loaded from the `extensions` folder inside the SufniBrowser directory.

## 🛠 Requirements
- **SufniBrowser** installed
- **.NET 6+ SDK** installed
- A **C# development environment** (e.g., Visual Studio)

## 🏗️ Creating an Extension
### 1️⃣ Create a New Class Library
1. Open **Visual Studio** and create a **Class Library (.NET 6+)** project.
2. Set the **Target Framework** to `.NET 6.0-windows` or newer.
3. Name the project, e.g., `MyExtension`.

### 2️⃣ Implement the `IBrowserExtension` Interface
SufniBrowser requires extensions to implement the following interface:

```csharp
using System.Windows.Forms;
using SufniVortex.Extensions;

namespace MyExtension
{
    public class MySampleExtension : IBrowserExtension
    {
        public void Initialize(Form browserForm)
        {
            MenuStrip menuStrip = browserForm.Controls.OfType<MenuStrip>().FirstOrDefault();
            if (menuStrip != null)
            {
                ToolStripMenuItem extensionMenuItem = new ToolStripMenuItem("My Extension");
                extensionMenuItem.Click += (sender, e) =>
                {
                    MessageBox.Show("Hello from My Extension!", "Extension Loaded");
                };
                menuStrip.Items.Add(extensionMenuItem);
            }
        }
    }
}
```

### 3️⃣ Add a Reference to `SufniVortex.Extensions`
1. **Right-click** on `Dependencies` in the Solution Explorer.
2. Select **Add Project Reference...**.
3. Locate and add the `SufniBrowser.dll` from the SufniBrowser installation folder.

### 4️⃣ Build the DLL
- **Press `Ctrl + Shift + B`** or go to **Build -> Build Solution**.
- Navigate to the **bin/Debug/net6.0-windows/** folder and copy the generated `.dll` file.

### 5️⃣ Deploy the Extension
1. Copy the `.dll` file into the `SufniBrowser/extensions/` folder.
2. Open `extensions.txt` in the same folder and add:
   ```
   MyExtension.dll
   ```
3. Restart **SufniBrowser**, and the extension should load automatically.

## 🧪 Testing the Extension
1. Launch **SufniBrowser**.
2. Open the **Menu** and look for the new menu item.
3. Click it to verify that the extension works.

## 🛠️ Debugging Tips
- Use `Console.WriteLine()` for logging (check the browser’s console output).
- Make sure your extension’s name in `extensions.txt` matches the `.dll` file name exactly.
- Check for missing dependencies if your extension fails to load.

## 🎉 Conclusion
Now you have successfully created and loaded an extension for **SufniBrowser**! Experiment by adding new features like custom UI elements, enhanced security, or ad-blocking.

