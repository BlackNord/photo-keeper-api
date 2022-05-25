import numpy as np
from typing import *
import cv2
import os
# import matplotlib.pyplot as plt

# Neural network model
proto_txt_ = os.path.join(os.getcwd(), 'Colorization', 'Models', 'colorization_deploy_v2.prototxt')
model_ = os.path.join(os.getcwd(), 'Colorization', 'Models', 'colorization_release_v2.caffemodel')
points_ = os.path.join(os.getcwd(), 'Colorization', 'Models', 'pts_in_hull.npy')

# Directories for input and output data
inputDir = os.path.join(os.getcwd(), 'Colorization', 'Input')
outputDir = os.path.join(os.getcwd(), 'Colorization', 'Output')


def gray_convert(elem):
    elem = cv2.cvtColor(elem, cv2.COLOR_BGR2GRAY)
    elem = cv2.cvtColor(elem, cv2.COLOR_GRAY2RGB)
    return elem


def load_model() -> Any:
    # L = lightness intensity
    # A = green-red
    # B = blue-yellow

    print("Loading...")

    # Load serialized b / w colorifier for model and cluster
    neuralNet = cv2.dnn.readNetFromCaffe(proto_txt_, model_)
    points = np.load(points_)

    # Load quantum bin centers, assign 1x1 kernels corresponding to each of the 313 bin cents
    # And we assign them to the appropriate level in the network. Next, add a scalable layer with a non-zero value
    class8 = neuralNet.getLayerId("class8_ab")
    conv8 = neuralNet.getLayerId("conv8_313_rh")
    points = points.transpose().reshape(2, 313, 1, 1)

    neuralNet.getLayer(class8).blobs = [points.astype("float32")]
    neuralNet.getLayer(conv8).blobs = [np.full([1, 313], 2.606, dtype="float32")]

    print("Loaded")

    return neuralNet


def colorize_image(neuralNet: Any, imageIn: str, imageOut: str):
    # Load image by filename
    image = cv2.imread(imageIn)
    # Initializing image parameters
    height, width, channels = image.shape

    # Erase with gray
    image = gray_convert(image)

    # Display b & w image
    # plt.imshow(image)
    # plt.show()

    # Extract layer L
    # Input RGB image is scaled, then converted to color
    # Lab space and luminance channel extracted
    scaled = image.astype("float32") / 255.0
    l = cv2.cvtColor(scaled, cv2.COLOR_RGB2LAB)

    # Casting L to the size the model is working with
    # Usually the luminance channel is in the range from 0 to 100. Therefore, you need to subtract a value close to 50,
    # To center it at 0
    resizedL = cv2.resize(l, (224, 224))
    L = cv2.split(resizedL)[0]
    L -= 47

    # Load brightness data into HH
    neuralNet.setInput(cv2.dnn.blobFromImage(L))
    # Revealing A, B
    AB = neuralNet.forward()[0, :, :, :].transpose((1, 2, 0))

    # Reset L to original size
    L = cv2.split(l)[0]

    # Return to previous size using saved parameters
    AB = cv2.resize(AB, (width, height))

    # Gluing L with A, B => getting a color image
    colorized = np.concatenate((L[:, :, np.newaxis], AB), axis=2)

    # Display Lab image
    # plt.imshow(colorized)
    # plt.show()

    # Cast to RGB
    colorized = cv2.cvtColor(colorized, cv2.COLOR_LAB2RGB)
    colorized = np.clip(colorized, 0, 1)
    colorized = (255 * colorized).astype("uint8")

    # Display RGB image
    # plt.imshow(colorized)
    # plt.show()

    # Writing the result to the image-output parameter
    cv2.imwrite(imageOut, cv2.cvtColor(colorized, cv2.COLOR_RGB2BGR))
    # print("Image %s saved" % imageOut)


if __name__ == '__main__':
    neuralNet = load_model()
    # We connect to the module as needed - work with images fragments
    for filename in os.listdir(inputDir):
        colorize_image(neuralNet, inputDir + os.sep + filename, outputDir + os.sep + filename)
