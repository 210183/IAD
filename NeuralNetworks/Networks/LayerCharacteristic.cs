namespace NeuralNetworks
{
    public class LayerCharacteristic
    {
        public int NumberOfNeurons { get; set; }
        public IOnGoingTrainer ActivationFunction { get; set; }
        public LayerCharacteristic(int numberOfNeurons, IOnGoingTrainer activationFunction)
        {
            this.NumberOfNeurons = numberOfNeurons;
            this.ActivationFunction = activationFunction;
        }
    }
}