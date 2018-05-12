namespace NeuralNetworks
{
    public class LayerCharacteristic
    {
        public int NumberOfNeurons { get; set; }
        public IActivationFunction ActivationFunction { get; set; }
        public LayerCharacteristic(int numberOfNeurons, IActivationFunction activationFunction)
        {
            this.NumberOfNeurons = numberOfNeurons;
            this.ActivationFunction = activationFunction;
        }
    }
}