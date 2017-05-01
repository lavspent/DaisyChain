﻿/*
    Copyright(c) 2017 Petter Labråten/LAVSPENT.NO. All rights reserved.

    The MIT License(MIT)

    Permission is hereby granted, free of charge, to any person obtaining a
    copy of this software and associated documentation files (the "Software"),
    to deal in the Software without restriction, including without limitation
    the rights to use, copy, modify, merge, publish, distribute, sublicense,
    and/or sell copies of the Software, and to permit persons to whom the
    Software is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
    FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
    DEALINGS IN THE SOFTWARE.
*/

using System;
using Windows.Devices.Gpio.Provider;

namespace Lavspent.DaisyChain.Gpio
{
    /// <summary>
    /// Wraps a DaisyChain IGpioController as an UWP IGpioControllerProvider.
    /// </summary>
    internal class UwpGpioControllerProvider : IGpioControllerProvider
    {
        private IGpioController _controller;

        public UwpGpioControllerProvider(IGpioController controller)
        {
            _controller = controller;
        }

        int IGpioControllerProvider.PinCount
        {
            get
            {
                return _controller.GpioCount;
            }
        }

        IGpioPinProvider IGpioControllerProvider.OpenPinProvider(int pinNumber, ProviderGpioSharingMode sharingMode)
        {
            if (sharingMode != ProviderGpioSharingMode.Exclusive)
                throw new NotSupportedException("Only supports Exclusive sharing mode.");

            IGpio gpio = AsyncInline.AsyncInline.Run(
                () => _controller.OpenGpioAsync(pinNumber));

            return new UwpGpioPinProvider(gpio);
        }
    }
}
