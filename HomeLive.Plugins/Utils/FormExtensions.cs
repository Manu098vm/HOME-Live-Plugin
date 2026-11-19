namespace HomeLive.Plugins;

public static class FormExtensions
{
    /// <summary>
    /// Shows a message box owned by <paramref name="form"/>, on its UI thread.
    /// Falls back to an unowned message box if the form has already been closed.
    /// </summary>
    public static void ShowMessageSafe(this Form form, string text, MessageBoxIcon icon)
    {
        try
        {
            if (!form.IsDisposed && form.IsHandleCreated)
            {
                if (form.InvokeRequired)
                    form.Invoke(() => MessageBox.Show(form, text, "HomeLive", MessageBoxButtons.OK, icon));
                else
                    MessageBox.Show(form, text, "HomeLive", MessageBoxButtons.OK, icon);
                return;
            }
        }
        catch (Exception ex) when (ex is ObjectDisposedException or InvalidOperationException)
        {
            // The form was closed in the meantime.
        }

        MessageBox.Show(text, "HomeLive", MessageBoxButtons.OK, icon);
    }
}
