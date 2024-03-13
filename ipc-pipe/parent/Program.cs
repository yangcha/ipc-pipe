using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace parent
{
    public static class ProcessExtensions
    {
        /// <summary>
        /// Returns a task that completes when the process exits and provides the exit code of that process.
        /// </summary>
        /// <param name="process">The process to wait for exit.</param>
        /// <param name="cancellationToken">
        /// A token whose cancellation will cause the returned Task to complete
        /// before the process exits in a faulted state with an <see cref="OperationCanceledException"/>.
        /// This token has no effect on the <paramref name="process"/> itself.
        /// </param>
        /// <returns>A task whose result is the <see cref="Process.ExitCode"/> of the <paramref name="process"/>.</returns>
        public static async Task<int> WaitForExitAsync(this Process process, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (process is null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            var tcs = new TaskCompletionSource<int>();
            EventHandler exitHandler = (s, e) =>
            {
                tcs.TrySetResult(process.ExitCode);
            };
            try
            {
                process.EnableRaisingEvents = true;
                process.Exited += exitHandler;
                if (process.HasExited)
                {
                    // Allow for the race condition that the process has already exited.
                    tcs.TrySetResult(process.ExitCode);
                }

                using (cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken)))
                {
                    return await tcs.Task.ConfigureAwait(false);
                }
            }
            finally
            {
                process.Exited -= exitHandler;
            }
        }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            int retCode = 0;
            using (Process childProcess =
                new Process
                {
                    StartInfo =
                    {
                        FileName = "child.exe",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardError = true,
                    },
                }
                    )
            {

                try
                {
                    childProcess.ErrorDataReceived += (sender, eventArgs) =>
                    {
                        Console.WriteLine($"{nameof(childProcess)},  {eventArgs.Data}");
                    };
                    childProcess.Start();
                    childProcess.BeginErrorReadLine();
                    var exitCode = await childProcess.WaitForExitAsync();
                    retCode = exitCode;
                }
                catch (OperationCanceledException)
                {
                    childProcess.Kill();
                }
                catch (ArgumentNullException a)
                {
                    System.Console.WriteLine(a.Message);
                }
                catch (Exception w)
                {
                    System.Console.WriteLine("here");
                }

            }
        }
    }
}
