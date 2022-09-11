using System.Threading.Channels;

var channel_options = new BoundedChannelOptions(1)
{
    SingleReader = true,
    SingleWriter = true,
    FullMode = BoundedChannelFullMode.Wait
};
var channel = Channel.CreateBounded<char>(channel_options);

var producer = Task.Run(async () =>
{
    await channel.Writer.WriteAsync('a');
    await channel.Writer.WriteAsync('b');   //commenting this will avoid a deadlock
});

var consumer = Task.Run(() =>
{
    channel.Reader.TryRead(out _);
});

await Task.WhenAll(producer, consumer);

Console.WriteLine("Finished!");